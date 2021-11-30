using MailKit.Net.Smtp;
using Microsoft.AspNet.Identity.EntityFramework;
using MimeKit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

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
    
        public void Notify(string email, string SubType)
        {
          
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Subscription confirmation", "teamosmium1@gmail.com"));
            message.To.Add(new MailboxAddress("Subscription confirmation", email));
            message.Subject = "BookMarked - "+SubType+" subscription is confirmed!";
            message.Body = new TextPart("plain")
            {
                Text = "Hello "+email+", you have successfully subscribed to the "+SubType+" membership of BookMarked."
            };
            using(var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("teamosmium1@gmail.com", "ASSK@123");
                
                client.Send(message);

                client.Disconnect(true);
            }

        }

    
    }
}
