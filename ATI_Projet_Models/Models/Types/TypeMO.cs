using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models.Types
{
    [DisplayName("Types Main d'oeuvre")]
    public class TypeMO
    {
        [Required(ErrorMessage ="Le code est requis")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Le libellé est requis")]
        public string Libelle { get; set; }
        [Required(ErrorMessage = "Le type de job est requis")]
        public int TypeJobId { get; set; }
        [Required(ErrorMessage = "Le prix de revient est requis")]
        public int PrixRevient { get; set; }
        [Required(ErrorMessage = "Le prix facture est requis")]
        public int PrixFacture { get; set; }
    }
}
