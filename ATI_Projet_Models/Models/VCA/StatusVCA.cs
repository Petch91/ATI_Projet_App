using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models.VCA
{
    [DisplayName("Status VCA")]
    public class StatusVCA
    {
        public int Code { get; set; }
        [Required(ErrorMessage = "Le Libellé est requis")]
        public string Libelle { get; set; }
    }
}
