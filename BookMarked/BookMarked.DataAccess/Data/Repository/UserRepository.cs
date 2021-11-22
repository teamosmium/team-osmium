using BookMarked.Data;
using BookMarked.DataAccess.Data.Repository;
using BookMarked.DataAccess.Repository.IRepository;
using BookMarked.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarked.DataAccess.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {

            _db = db;
        }


    }
}