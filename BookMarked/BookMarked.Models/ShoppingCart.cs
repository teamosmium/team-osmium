using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarked.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            
        }
        [Key]
        public int id { get; set; }
        public string  UserId{ get; set; }
        [ForeignKey("UsereId")]
        
        public string ProductId { get; set; }
        [ForeignKey("Product")]
        public Product Product { get; set; }
        public int count { get; set; }
        [NotMapped]
        public double Price { get; set; }

    }
}
