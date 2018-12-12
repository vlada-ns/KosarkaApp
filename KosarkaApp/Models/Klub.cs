using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KosarkaApp.Models
{
    public class Klub
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Unesite tekstualnu vrednost sa najvise 50 karaktera.")]
        public string Naziv { get; set; }

        [StringLength(3, MinimumLength = 3, ErrorMessage = "Unesite tekstualnu vrednost sa tacno 3 karaktera.")]
        public string Liga { get; set; }

        [Required]
        [Range(1945, 1999, ErrorMessage = "Unesite celobrojnu vrednost iz intervala 1945 - 2000.")]
        public int Godina { get; set; }

        [Required]
        [Range(0, 19, ErrorMessage = "Unesite celobrojnu vrednost veću ili jednaku 0, a manju od 20.")]
        public int Trofeji { get; set; }
    }
}