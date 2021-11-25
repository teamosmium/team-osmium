using BookMarked.Data;
using BookMarked.Models;
using BookMarked.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarked.DataAccess.Data.Repository
{
    public class EBookRepository
    {
        private ApplicationDbContext _context = null;
        public EBookRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<EBook> GetAllEBooks()
        {
            return _context.EBooks
                          .Select(eBook => new EBook()
                          {
                              Author = eBook.Author,
                              Category = eBook.Category,
                              Description = eBook.Description,
                              EBookId = eBook.EBookId,
                              Title = eBook.Title,
                              CoverImageUrl = eBook.CoverImageUrl,
                              BookPdf=eBook.BookPdf,
                              Price=eBook.Price
                          }).ToList();

        }
        public EBook GetEBookById(int id)
        {
            return _context.EBooks.Where(x => x.EBookId == id)
                 .Select(eBook => new EBook()
                 {
                     Author = eBook.Author,
                     Category = eBook.Category,
                     Description = eBook.Description,
                     EBookId = eBook.EBookId,
                     Title = eBook.Title,
                     CoverImageUrl = eBook.CoverImageUrl,
                     BookPdfUrl = eBook.BookPdfUrl,
                     Price=eBook.Price
                 }).FirstOrDefault();
        }
        public List<EBook> SearchEBook(string title, string authorName)
        {
            return DataSource().Where(x => x.Title == title || x.Author == authorName).ToList();
        }
        public async Task<int> AddNewEBook(EBookVM eBook)
        {
            var newEBook = new EBook()
            {
                Author = eBook.EBook.Author,
                Description = eBook.EBook.Description,
                Title = eBook.EBook.Title,
                CoverImageUrl = eBook.EBook.CoverImageUrl,
                BookPdfUrl = eBook.EBook.BookPdfUrl,
                CategoryId=eBook.EBook.CategoryId,
                Price=eBook.EBook.Price
 
            };

            await _context.EBooks.AddAsync(newEBook);
            await _context.SaveChangesAsync();

            return newEBook.EBookId;

        }
        private List<EBook> DataSource()
        {
            return new List<EBook>()
            {
                new EBook(){EBookId =1,Title="MVC", Author="S", CategoryId=1},
                new EBook(){EBookId =2,Title="SCDSC2", Author="S",CategoryId=2},
                new EBook(){EBookId =3,Title="SCDSC3", Author="S",CategoryId=2},
                new EBook(){EBookId =4,Title="SCDSC4", Author="S",CategoryId=1},
                new EBook(){EBookId =5,Title="SCDSC5", Author="S",CategoryId=1},
            };
        }
    }
}
