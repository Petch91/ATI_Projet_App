using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models.Models.Projets
{
   public class Projet
   {
      public int Id { get; set; }
      public int Code { get; set; }
      public bool TypeMarche { get; set; }

      public string Designation { get; set; }
      public string? TypeFacturation { get; set; }
      public string? NumCahierCharges { get; set; }
      public string? NumCommandeClient { get; set; }
      public string Planning {  get; set; }

      public DateTime DateDebut { get; set; }
      public DateTime DateFin { get;  set; }

      public int KM { get; set; }

      public int ClientId { get; set; }
      public int AmId { get; set; }
      public int RespProjetId { get; set; }
      public int RespAffaireId { get; set; }
      public int RespFacturationId { get;set; }
      public int SpId { get; set; }
      public int DeptId { get; set;}
      public int AdresseId { get; set; }
   }
}
