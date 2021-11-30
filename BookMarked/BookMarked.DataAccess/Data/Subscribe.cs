using BookMarked.Data;
using BookMarked.DataAccess.Data.Repository;
using BookMarked.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarked.DataAccess.Data
{
    public class Subscribe : Repository<Subscription>, ISubscribe
    {
        private ApplicationDbContext _context = null;
        private User _observer = new User();
        public Subscribe(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Notify()
        {
            _observer.Notify();

        }

        public void registerObserver(string observerId,string subType)
        {
            User user = _context.User.FirstOrDefault(x=>x.UserId==observerId);
            user.IsSubscribed = true;
            //Add subscription data to table
            Subscription subscription = new Subscription();
            subscription.UserId = user.UserId;
            subscription.SubscriptionType = subType;
            subscription.SubscriptionStartDate = DateTime.Now;
            subscription.SubscriptionEndDate = DateTime.Now.AddDays(10);
            _context.Add(subscription);
            _context.SaveChanges();
            Notify();
        }

        public void unregisterObserver(User observer)
        {
            throw new NotImplementedException();
        }
    }
}
