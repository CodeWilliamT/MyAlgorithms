using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_SubTools.Models
{
    public class Model_Sub_Switcher
    {
        [Required]
        [Display(Name = "Sub File")]
        public IFormFile SubFormFile { get; set; }
    }
}
