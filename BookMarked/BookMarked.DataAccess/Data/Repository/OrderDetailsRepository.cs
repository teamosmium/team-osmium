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
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {

            _db = db;
        }

        public void Update(OrderDetails orderDetail)
        {
            _db.OrderDetails.Update(orderDetail);
        }
    }
}
