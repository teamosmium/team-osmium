using BookMarked.DataAccess.Data;
using BookMarked.DataAccess.Data.Repository;
using BookMarked.DataAccess.Data.Repository.IRepository;
using BookMarked.Models;
using BookMarked.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookMarked.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EBookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EBookRepository _eBookRepository= null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Subscribe _subscribe;
        public EBookController(EBookRepository eBookRepository, IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork, 
            Subscribe subscribe)
        {
            _eBookRepository = eBookRepository;
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
            _subscribe = subscribe;
        }

        public IActionResult Index()
        {
            IEnumerable<EBook> eBookList = _eBookRepository.GetAllEBooks();
            return View(eBookList);
        }

        public IActionResult GetAllBooks()
        {
            var data = _eBookRepository.GetAllEBooks();
            return View(data);
        }
        public IActionResult GetEBook(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            User user = new User();
            var activeUser = _unitOfWork.User.GetFirstOrDefault(x=>x.UserId==claim.Value);
            var data = _eBookRepository.GetEBookById(id);
            ViewBag.IsSubscribed = activeUser.IsSubscribed;
            return View(data);
        }
        public List<EBook> SearchBooks(string bookName, string authorName)
        {
            return _eBookRepository.SearchEBook(bookName, authorName);
        }
        public IActionResult AddNewBook(int? id)
        {
            
            EBookVM eBookVM = new EBookVM()
            {
                  EBook = new EBook(),
                  CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.CategoryId.ToString()
                })
            };
            if (id == null)
            {
                return View(eBookVM);
            }
            eBookVM.EBook = _eBookRepository.GetEBookById(id.GetValueOrDefault());
            if (eBookVM.EBook == null)
            {
                return NotFound();
            }
            return View(eBookVM);
        }
        [HttpPost]
        public async Task<IActionResult> AddNewBook(EBookVM ebookvm)
        {
            if (ModelState.IsValid)
            {
                if (ebookvm.EBook.CoverPhoto != null)
                {
                    string folder = "images/ebook/";
                    ebookvm.EBook.CoverImageUrl = await UploadImage(folder, ebookvm.EBook.CoverPhoto);
                }

                if (ebookvm.EBook.BookPdf != null)
                {
                    string folder = "ebooks/pdf/";
                    ebookvm.EBook.BookPdfUrl = await UploadImage(folder, ebookvm.EBook.BookPdf);
                }

                int id = await _eBookRepository.AddNewEBook(ebookvm);
                if (id > 0)
                {
                    return RedirectToAction(nameof(AddNewBook), new { isSuccess = true, bookId = id });
                }
            }

            return View(ebookvm);
        }

        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folderPath;
        }
        public IActionResult ViewPDF(int id)
        {
            var data = _eBookRepository.GetEBookById(id);

            return View(data);

        }
        public IActionResult SubscriptionPlan()
        {
            return View();
        }
        [HttpPost]
        [ActionName("SubscriptionPlan")]
        public IActionResult SubscriptionPlan(int id, bool isSubscribed,string stripeToken = null)
        {
            var data = _eBookRepository.GetEBookById(id);
            if (stripeToken != null)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
               
                _subscribe.registerObserver(claim.Value, "Gold");

                return RedirectToAction("Index", "Home", new { area = "User" });

            }
            return View(data);
        }
    }
}
