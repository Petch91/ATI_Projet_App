using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models
{
    public class EmployePrivate : ICloneable
    {
        public int Id { get; set; }
        public string? Langue { get; set; }
        [DisplayName(@"Nationalité")]
        public string? Nationalite { get; set; }
        [DisplayName("Date de Naissance")]
        public DateTime DateNaissance { get; set; }
        [DisplayName(@"Numéro National")]
        public string? NumeroNational { get; set; }
        public string? Signature { get; set; } 
        public string? Photo { get; set; } 
        public string? IBAN { get; set; }
        public string? Permis { get; set; }
        public int? AdresseId { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
