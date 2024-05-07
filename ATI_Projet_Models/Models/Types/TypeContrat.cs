using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models.Types
{
    [DisplayName("Types de Contrat")]
    public class TypeContrat
    {
        public int Code { get; set; }
        [Required(ErrorMessage ="Le libellé est obligatoire")]
        public string Libelle { get; set; }
    }
}
