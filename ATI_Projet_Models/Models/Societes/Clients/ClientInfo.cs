using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models.Models.Societes.Clients
{
   public class ClientInfo
   {
      public int Id {  get; set; }
      public string Name { get; set; }

      public bool Actif {  get; set; }
      public bool Conges { get; set; }
      public bool Test { get; set; }
      public bool SansCRM { get; set; }
      public bool SuiviJournalier { get; set; }

      public int AmId { get; set; }
      public int DeptId { get; set; }
      public int SocieteId { get; set; }
   }
}
