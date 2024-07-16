using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models.Models.Societes.Clients
{
   public class ClientSigna
   {
      public int Id { get; set; }
      public string? Vendeur { get; set; }
      public string TypePaiement { get; set; }
      public string RC { get; set; }
      public string TVA { get; set; }
      public int KM { get; set; }
      public string SiteInternet { get; set; }
      public string CompteBancaire { get; set; }
   }
}
