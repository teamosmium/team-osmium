using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarked.Models.ViewModels
{
    public class EBookVM
    {
        public EBook EBook { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
