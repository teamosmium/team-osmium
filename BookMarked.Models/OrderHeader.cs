﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarked.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id {  get; set; }
        public string UserId {  get; set; }
        [ForeignKey("UserId")]
        public User User {  get; set; }
        [Required]
        public DateTime OrderDate {  get; set;}
        [Required]
        public DateTime ShippingDate{  get; set; }
        [Required]
        public Double OrderTotal {  get; set; }
        public string TrackingNumber {  get; set; }
        public string carrier {  get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public string TransactionId { get; set; }
        public string PhoneNumber { get; set; }
        public string streetAdress { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string PostalCode { get; set; }
        public string Name {  get; set; }


    }
}
