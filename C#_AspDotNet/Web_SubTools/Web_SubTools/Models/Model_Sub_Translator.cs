using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_SubTools.Models
{
    public class Model_Sub_Translator
    {
        [Required]
        [Display(Name = "Sub File")]
        public IFormFile SubFormFile { get; set; }
        [Required]
        [Display(Name = "Translate From")]
        public string From { get; set; }
        [Required]
        [Display(Name = "Translate To")]
        public string To { get; set; }
    }
}
