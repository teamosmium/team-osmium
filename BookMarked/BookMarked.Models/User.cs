using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMarked.Models
{
    public class User:IUserObserver
    {
        [Key]
        public string UserId { get; set; }
        [Required]
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        [NotMapped]
        public string Role { get; set; }
        public bool IsSubscribed { get; set; }
    
        public void Notify()
        {
            var msg="ZHALA BHAVA!!";
        }
    }
}
