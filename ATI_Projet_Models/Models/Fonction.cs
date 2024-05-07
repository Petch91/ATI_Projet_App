using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models
{
    [DisplayName("Fonctions")]
    public class Fonction
    {
        public int Id {  get; set; }
        [Required(ErrorMessage = "La désignation est requis")]
        public string Designation { get; set; }
        public string? Abrege { get; set; }
    }
}
