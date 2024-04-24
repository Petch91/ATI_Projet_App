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
        [Required(ErrorMessage ="La rue et le numéro sont requis")]
        public string adresse { get; set; }
        [Required(ErrorMessage ="Code Postal Requis")]
        [Range(1000,9999,ErrorMessage ="Ceci n'est pas un code postal valide")]
        public int CodePostal { get; set; }
        [Required(ErrorMessage ="Une localité est requise")]
        public string Localite { get; set; }
        [Required(ErrorMessage ="Un Pays est requis")]
        public string Pays { get; set; }
        public int EmployeId { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
