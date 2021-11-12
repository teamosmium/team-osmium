using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarked.Models
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public string SubscriptionType { get; set; }
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        
    }
}
