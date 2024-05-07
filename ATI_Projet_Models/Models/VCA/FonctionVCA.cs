using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models.VCA
{
    [DisplayName("Fonctions VCA")]
    public class FonctionVCA
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Le posit est requis")]
        public int Posit { get; set; }
        [Required(ErrorMessage = "La désignation est requis")]
        public string? Designation { get; set; }
    }
}
