using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models
{
    public class Adresse : ICloneable
    {
        public int Id {  get; set; }
        [Required]
        public string adresse { get; set; }
        [Required]
        [Range(1000,9999)]
        public int CodePostal { get; set; }
        [Required]
        public string Localite { get; set; }
        [Required]
        public string Pays { get; set; }
        public int EmployeId { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
