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
        public int Id { get; set; }
        public string  UserId{ get; set; }
        [ForeignKey("UsereId")]
        
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int Count { get; set; }
        [NotMapped]
        public double Price { get; set; }

    }
}
