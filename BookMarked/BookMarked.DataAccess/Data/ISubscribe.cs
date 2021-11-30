using BookMarked.DataAccess.Data.Repository.IRepository;
using BookMarked.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarked.DataAccess.Data
{
    public interface ISubscribe : IRepository<Subscription>
    {
        void registerObserver(string observerId,string subType);
        void unregisterObserver(Subscription observer);
        void Notify(string email,string subType);
    }
}
