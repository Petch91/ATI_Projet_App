using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models.Models.Projets
{
   public class ProjetBC14
   {
      public int Id { get; set; }
      public string Designation { get; set; }
      public string ClientName { get; set; }

      public int RespFacturationId { get; set; }
      public int ClientId { get; set; }
      public string? ImpNumb { get; set; }
      public string CompNumber
      {
         get
         {
            if (string.IsNullOrEmpty(ImpNumb))
            {
               return ClientId.ToString("0000") + Id.ToString("0000");
            }
            return ImpNumb;
         }
      }
   }
}
