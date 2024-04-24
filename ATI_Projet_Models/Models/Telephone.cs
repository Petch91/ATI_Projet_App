using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models
{
    public class Telephone : ICloneable
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Le numero est requis")]
        [RegularExpression(@"^[+0][0-9]*$", ErrorMessage = "Le numéro n'est pas valide")]
        public string Numero { get; set; }
        [Required(ErrorMessage ="Description requise")]
        public string Description { get; set; }
        public int? PersonneId { get; set; }
        public int? SocieteId { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
