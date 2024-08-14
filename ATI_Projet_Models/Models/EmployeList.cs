using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projets_Models
{
   public class EmployeList
   {
      public int Id { get; set; }
      [DisplayName(@"Prénom")]
      public string? Prenom { get; set; }
      public string? Nom { get; set; }
      public bool Actif { get; set; }

      public string FullName
      {
         get
         {
            return Nom + " " + Prenom;
         }
      }
   }
}