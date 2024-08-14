using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models.Models.Projets
{
   public class FicheBC14
   {
      public string No { get; set; }
      public string Description { get; set; }
      public string Person_Responsible { get; set; }
      public string Responsable_Nom { get; set; }
      public string CompNumber
      {
         get
         {
            if (No.Length >= 8)
            {
               return No.Substring(No.Length - 8);
            }
            return No;
         }
      }
   }
}
