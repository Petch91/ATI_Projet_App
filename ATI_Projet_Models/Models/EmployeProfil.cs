using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models
{
    public class EmployeProfil :ICloneable
    {
        public int Id { get; set; }
        [Required]
        public string? Nom { get; set; }
        [Required]
        [DisplayName("Prénom")]
        public string? Prenom { get; set; }

        public string? Initiales { get; set; }
        public string? Titre { get; set; }
        public bool Actif { get; set; }
        public bool Status { get; set; }
        public int Group_S { get; set; }
        [DisplayName("Date d'entrée")]
        public DateTime DateEntree { get; set; }
        [DisplayName("Date de sortie")]
        public DateTime DateSortie { get; set; }
        public int GestionStock { get; set; }
        public int IdFonctionVCA { get; set; }
        public int CodeStatusVCA { get; set; }
        public int CodeTypeContrat { get; set; }
        public string? CodeTypeMO { get; set; }
        public int DepartementId { get; set; }
        public int FonctionId { get; set; }
        public int PersonneId { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
