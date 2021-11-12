using BookMarked.Data;
using BookMarked.DataAccess.Data.Repository.IRepository;
using BookMarked.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarked.DataAccess.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {

            _db = db;
        }

        public void Update(Product product)
        {
            var objFromDb = _db.Products.FirstOrDefault(s => s.ProductId == product.ProductId);
            if (objFromDb != null)
            {
                if (product.ImageURL != null)
                {
                    objFromDb.ImageURL = product.ImageURL;
                }
                objFromDb.ISBN = product.ISBN;
                objFromDb.Price = product.Price;
                objFromDb.Title = product.Title;
                objFromDb.Description = product.Description;
                objFromDb.CategoryId = product.CategoryId;
                objFromDb.Author = product.Author;
            }
        }
    }
}
