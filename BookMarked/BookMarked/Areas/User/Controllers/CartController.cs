using BookMarked.DataAccess.Data.Repository.IRepository;
using BookMarked.Models;
using BookMarked.Models.ViewModels;
using BookMarked.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookMarked.Areas.Customer.Controllers
{
    [Area("User")]
    public class CartController : Controller

    {
        private readonly IUnitOfWork _unitOfWork;
        private UserManager<IdentityUser> _userManager;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader(),
                ListCart = _unitOfWork.ShoppingCart.GetAll(x => x.UserId == claim.Value, includePropreties: "Product"),
            };

            ShoppingCartVM.OrderHeader.OrderTotal = 0;

            foreach (var item in ShoppingCartVM.ListCart)
            {
                item.Price = item.Count * item.Product.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += item.Price * item.Count;
                item.Product.Description = SD.ConvertToRawHtml(item.Product.Description);

                if (item.Product.Description.Length > 100)
                {
                    item.Product.Description = item.Product.Description.Substring(0, 99) + "...";
                }

            }

            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId, includePropreties: "Product");
            cart.Count += 1;
            cart.Price = (cart.Count * cart.Product.Price);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId, includePropreties: "Product");
            if (cart.Count == 1)
            {
                var cnt = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == cart.UserId).ToList().Count();
                _unitOfWork.ShoppingCart.Remove(cart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.ssShoppingCart, cnt - 1);
            }

            else
            {

                cart.Count -= 1;
                cart.Price = cart.Count * cart.Product.Price;
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));

        }



        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId, includePropreties: "Product");
            var cnt = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == cart.UserId).ToList().Count();
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.ssShoppingCart, cnt - 1);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary(int cartId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
             ShoppingCartVM = new ShoppingCartVM()
            {

                OrderHeader = new OrderHeader(),
                ListCart = _unitOfWork.ShoppingCart.GetAll(c => c.UserId == claim.Value,includePropreties:"Product")
            };

            foreach (var item in ShoppingCartVM.ListCart)
            {
                item.Price = (item.Count * item.Product.Price);
                ShoppingCartVM.OrderHeader.OrderTotal += item.Price * item.Count;
            }

            ShoppingCartVM.OrderHeader.Name = User.Identity.Name;

            return View (ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPost(string stripeToken) 
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll(c => c.UserId == claim.Value, includePropreties: "Product");
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.OrderHeader.UserId = claim.Value;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            foreach (var item in ShoppingCartVM.ListCart)
            {
                item.Price = (item.Count * item.Product.Price);
                OrderDetails orderDetails = new()

                {
                    ProductId = item.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price=item.Price,
                    Count=item.Count

                };
                ShoppingCartVM.OrderHeader.OrderTotal += orderDetails.Count * orderDetails.Price;
                _unitOfWork.OrderDetails.Add(orderDetails);
           
                    var prod = _unitOfWork.Product.Get(orderDetails.ProductId);
                    prod.Sales = prod.Sales + orderDetails.Count;
                    _unitOfWork.Save();
                
            }
            _unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.ssShoppingCart, 0);


            if (stripeToken != null)
            {

                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(ShoppingCartVM.OrderHeader.OrderTotal * 100),
                    Currency = "eur",
                    Description = $"Order ID :{ShoppingCartVM.OrderHeader.Id}",
                    Source = stripeToken
                };

                var service = new ChargeService();
                Charge charge = service.Create(options);

                if (charge.BalanceTransactionId == null)
                {
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                }
                else 
                {
                    ShoppingCartVM.OrderHeader.TransactionId = charge.BalanceTransactionId;
                }

                if (charge.Status.ToLower() == "succeeded") 
                {
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;

                }

            }

            _unitOfWork.Save();
            return RedirectToAction("OrderConfirmation", "Cart", new { id = ShoppingCartVM.OrderHeader.Id });

            
        }

        public IActionResult OrderConfirmation(int Id) 
        
        {
            return View(Id);
        }
    }
}
