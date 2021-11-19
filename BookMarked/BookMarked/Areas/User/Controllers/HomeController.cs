using BookMarked.DataAccess.Data.Repository.IRepository;
using BookMarked.Models;
using BookMarked.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includePropreties: "Category");
            return View(productList);
        }
        public IActionResult Details(int id)

        {
            var productFromDb= _unitOfWork.Product.GetFirstOrDefault(u=>u.ProductId == id, includePropreties:"Category");
            ShoppingCart cartobj = new ShoppingCart()
            {
                Product = productFromDb
            };
            return View(cartobj);
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
