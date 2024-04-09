using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models
{
    public class Adresse
    {
        public int Id {  get; set; }
        public string adresse { get; set; }
        public int CodePostal { get; set; }
        public string Localite { get; set; }
        public string Pays { get; set; }
        public int EmployeId { get; set; }

    }
}
