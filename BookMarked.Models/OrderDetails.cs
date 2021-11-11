using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarked.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id {  get; set; }
        [Required]
        public int OrderId {  get; set; }
        [ForeignKey("Order Id")]
        public OrderHeader OrderHeader {  get; set; }

        public int productId { get; set; }
        [ForeignKey("Product Id")]
        public Product Product { get; set; }
        public int count { get; set; }
        public double Price { get; set; }

    }
}
