using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using BookMarked.Models;
using BookMarked.DataAccess.Data.Repository.IRepository;
using BookMarked.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using BookMarked.DataAccess.Data.Repository;
using System.Collections.Generic;

namespace BookMarked.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ProductRepository _productRepository = null;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, ProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _productRepository.GetAllProducts();
            return View(productList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.CategoryId.ToString()
                })
            };
            if (id == null)
            {
                return View(productVM);
            }
            productVM.Product = _unitOfWork.Product.Get(id.GetValueOrDefault());
            if (productVM.Product == null)
            {
                return NotFound();
            }
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\products");
                    var extension = Path.GetExtension(files[0].FileName);
                    if (productVM.Product.ImageURL != null)
                    {
                        //this is an edit, we need to remove old image

                        var imagePath = Path.Combine(webRootPath, productVM.Product.ImageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    productVM.Product.ImageURL = @"\images\products\" + fileName + extension;
                }
                else
                {
                    //update when no image change
                    if (productVM.Product.ProductId != 0)
                    {
                        Product objFromDb = _unitOfWork.Product.Get(productVM.Product.ProductId);
                        productVM.Product.ImageURL = objFromDb.ImageURL;
                    }
                }
                if (productVM.Product.ProductId == 0)
                {
                    productVM.Product.CreatedOn = DateTime.Now;
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(productVM);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Product.GetAll(includePropreties: "Category");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Product.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            //this is an edit, we need to remove old image
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, objFromDb.ImageURL.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }


            _unitOfWork.Product.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion

    }
}