using BookMarked.DataAccess.Data.Repository.IRepository;
using BookMarked.Models;
using BookMarked.Models.ViewModels;
using BookMarked.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookMarked
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ProductVM productVM = new ProductVM();
            productVM.productList = _unitOfWork.Product.GetAll(includePropreties: "Category");

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(c => c.UserId == claim.Value).ToList().Count();
                HttpContext.Session.SetInt32(SD.ssShoppingCart, count);
            }
            productVM.TrendingProds = productVM.productList.OrderByDescending(x => x.Sales).Take(4);
            productVM.newReleases = productVM.productList.OrderByDescending(x => x.CreatedOn).Take(4);
            return View(productVM);
        }
        public IActionResult Details(int id)

        {
            var productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.ProductId == id, includePropreties: "Category");
            ShoppingCart cartobj = new ShoppingCart()
            {
                ProductId = productFromDb.ProductId,
                Product = productFromDb
            };
            return View(cartobj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart cartObject)
        {
            cartObject.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                cartObject.UserId = claim.Value;

                ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.UserId == cartObject.UserId
                && u.ProductId == cartObject.ProductId);

                if (cartFromDb == null)
                {
                    //no record on DB 
                    _unitOfWork.ShoppingCart.Add(cartObject);
                }
                else
                {
                    cartFromDb.Count += cartObject.Count;
                    _unitOfWork.ShoppingCart.Update(cartFromDb);
                }

                _unitOfWork.Save();
                var count = _unitOfWork.ShoppingCart.GetAll(c => c.UserId == cartObject.UserId).ToList().Count();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.ProductId == cartObject.ProductId, includePropreties: "Category");
                ShoppingCart cartObj = new ShoppingCart()
                {
                    Product = productFromDb,
                    ProductId = productFromDb.ProductId

                };

                return View(cartObj);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}