﻿using BookMarked.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarked.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string  ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Range(1,10000)]
        public double Price { get; set; }
        public string ImageURL { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

    }
}
