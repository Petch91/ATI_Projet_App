using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models.Models.Forms
{
    public class PersonneForm
    {
        [Required]
        public string Titre { get; set; }
        [Required]
        public string Prenom { get; set; }
        [Required]
        public string Nom { get; set; }
        public string? Langue { get; set; }
        public string? Nationalite { get; set; }
    }
}
