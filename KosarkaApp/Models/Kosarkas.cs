using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KosarkaApp.Models
{
    public class Kosarkas
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Unesite tekstualnu vrednost sa najvise 40 karaktera.")]
        public string   Naziv { get; set; }

        [Required]
        [Range(1976, 1999, ErrorMessage = "Unesite celobrojnu vrednost manju od 2000, a vecu od 1975.")]
        public int Godina { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Unesite celobrojnu vrednost vecu od 0.")]
        public int BrojUtakmica { get; set; }

        [Required]
        [Range(0.00001, 29.99999, ErrorMessage = "Decimalna vrednost veca od 0 a manja od 30.")]
        public decimal ProsecanBrojPoena { get; set; }

        public int KlubId { get; set; }
        public Klub klub { get; set; }

    }
}