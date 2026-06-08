using System.ComponentModel;

namespace ATI_Projet_Models.Models.Projets
{
   public class CompStatutBC14
   {
      [DisplayName(@"ID BC14")]
      public string No { get; set; }

      [DisplayName(@"Nom du client")]
      public string ClientName { get; set; }

      [DisplayName(@"Nom projet ATI")]
      public string Designation { get; set; }

      [DisplayName(@"Nom projet BC14")]
      public string Description { get; set; }

      [DisplayName(@"Statut ATI")]
      public string StatutATI { get; set; }

      [DisplayName(@"Statut EEB (BC14)")]
      public string StatutEEB { get; set; }

      [DisplayName(@"Statut BC14")]
      public string StatutBC14 { get; set; }

      [DisplayName(@"Numéro utilisé pour comparer (ATI)")]
      public string CompNumberATI { get; set; }

      [DisplayName(@"Numéro utilisé pour comparer (BC14)")]
      public string CompNumberBC { get; set; }

      public bool IsStatutMismatch { get; set; }
   }
}
