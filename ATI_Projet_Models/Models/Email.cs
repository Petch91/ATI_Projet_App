using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models
{
    public class Email : ICloneable
    {
        public int Id { get; set; }
        [EmailAddress(ErrorMessage ="Ce n'est pas une adresse mail valide")]
        [Required(ErrorMessage ="Adresse Mail requise")]
        [DisplayName("Adresse")]
        public string email { get; set; }
        [Required(ErrorMessage ="Description requise")]
        public string? Description { get; set; }
        public int? PersonneId { get; set; }
        public int? SocieteId { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
