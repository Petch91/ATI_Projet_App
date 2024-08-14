using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models.Models.Projets
{
   public class CompBC14
   {
      public int Index { get; set; }
      public string No { get; set; }
      public string Description { get; set; }
      public string Person_Responsible { get; set; }
      public string CompNumberBC { get; set; }

      public string Designation { get; set; }
      public string ClientName { get; set; }

      public int RespFacturationId { get; set; }
      public string RespATI { get; set; }
      public string RespBC14 { get; set; }
      public string? ImpNumb { get; set; }
      public string CompNumberATI { get; set; }
      public int NewPerson {  get; set; }
   }
}
