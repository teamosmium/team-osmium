﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarked.Models
{
    public class User : IdentityUser

    { 
        public string Name { get; set;  }
        public string StreetAddress {  get; set; }
        public string City {  get; set; }
        public string State {  get; set; }
        public string PostalCode {  get; set; }

    }
}
