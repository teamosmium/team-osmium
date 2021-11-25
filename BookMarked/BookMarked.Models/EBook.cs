using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarked.Models
{
    public class EBook
    {
        [Key]
        public int EBookId { get; set; }
        [StringLength(100, MinimumLength = 5)]
        [Required(ErrorMessage = "Please enter the title of your book")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter the author name")]
        public string Author { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public int Price { get; set; }

        [Display(Name = "Choose the cover photo of your book")]
        [NotMapped]
        public IFormFile CoverPhoto { get; set; }

        public string CoverImageUrl { get; set; }


        [Display(Name = "Upload your book in pdf format")]
        [NotMapped]
        public IFormFile BookPdf { get; set; }
 
        public string BookPdfUrl { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }


    }
}
