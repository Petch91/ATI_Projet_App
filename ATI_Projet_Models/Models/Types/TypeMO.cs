using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models.Types
{
    public class TypeMO
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
        public int TypeJobId { get; set; }
        public int PrixRevient { get; set; }
        public int PrixFacture { get; set; }
    }
}
