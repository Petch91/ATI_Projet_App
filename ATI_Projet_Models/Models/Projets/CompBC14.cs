using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models.Models.Projets
{
   public class CompBC14
   {
      public int Index { get; set; }
      [DisplayName(@"ID BC14")]
      public string No { get; set; }

      [DisplayName(@"Nom du client")]
      public string ClientName { get; set; }
      [DisplayName(@"Nom projet ATI")]
      public string Designation { get; set; }
      [DisplayName(@"Nom projet BC14")]
      public string Description { get; set; }

      [DisplayName(@"Id Responsable ATI")]
      public int RespFacturationId { get; set; }
      [DisplayName(@"Nom responsable ATI")]
      public string RespATI { get; set; }
      [DisplayName(@"ID Responsable BC14")]
      public string Person_Responsible { get; set; }
      [DisplayName(@"Nom responsable BC14")]
      public string RespBC14 { get; set; }

      [DisplayName(@"Numéro utilisé pour comparer (ATI)")]
      public string CompNumberATI { get; set; }
      [DisplayName(@"Numéro utilisé pour comparer (BC14)")]
      public string CompNumberBC { get; set; }

      [DisplayName(@"Id nouveau responsable choisi")]
      public int NewPerson {  get; set; }
      [DisplayName(@"Nom nouveau responsable choisi")]
      public string NewPersonName { get; set; }
   }
}
