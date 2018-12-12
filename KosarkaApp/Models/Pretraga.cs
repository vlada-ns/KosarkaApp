using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KosarkaApp.Models
{
    public class Pretraga
    {
        [Required]
        public int Najmanje { get; set; }
        [Required]
        public int Najvise { get; set; }
    }
}