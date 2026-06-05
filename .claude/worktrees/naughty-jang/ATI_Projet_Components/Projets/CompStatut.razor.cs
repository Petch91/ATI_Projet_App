using ATI_Projet_Cultures.Locales;
using ATI_Projet_Cultures.Tools;
using ATI_Projet_Models.Models.Projets;
using ATI_Projet_Tools.Services.Interfaces;
using ATI_Projets_Models;
using BlazorBootstrapPerso;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace ATI_Projet_Components.Projets;

public partial class CompStatut : ComponentBase, IDisposable
{
   [Inject] private IProjet _projet { get; set; }
   [Inject] private IPersonnel _personnel { get; set; }
   [Inject] private IStringLocalizer<PersonnelResource> localizer { get; set; }
   [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

   private IEnumerable<ProjetBC14> ProjetsATI { get; set; }
   private IEnumerable<FicheBC14> ProjetsBC14 { get; set; }
   private IEnumerable<EmployeList> employeList { get; set; }
   private IEnumerable<StatutProjet> statutsATI { get; set; }

   private List<CompStatutBC14> CompList = new List<CompStatutBC14>();

   private Grid<CompStatutBC14> grid = new Grid<CompStatutBC14>();
   private BlazorBootstrapPerso.Modal modal;

   private bool isOK = false;
   private int pageSize = 15;

   // Mapping ATI StatutProjet.Id → BC14 Status string
   // À configurer selon vos données
   private static readonly Dictionary<int, string> StatutATIversBC14 = new()
   {
      // { 1, "Open" },
      // { 2, "Planning" },
      // { 3, "In Process" },
      // { 4, "Completed" },
   };

   protected async override Task OnInitializedAsync()
   {
      LanguageNotifier.SubscribeLanguageChange(this);
      LanguageNotifier.SubscribeLanguageChange(grid);

      ProjetsATI = await _projet.GotProjetsBc14();
      ProjetsBC14 = await _projet.GotAllFichesBc14();
      employeList = await _personnel.GotPersonnelList();
      statutsATI = await _projet.GotStatuts();

      Compare();

      isOK = true;
   }

   public void Dispose()
   {
      LanguageNotifier.UnsubscribeLanguageChange(this);
      LanguageNotifier.UnsubscribeLanguageChange(grid);
   }

   private string GetStatutATIDesignation(int spId)
   {
      var statut = statutsATI.FirstOrDefault(s => s.Id == spId);
      return statut?.Designation ?? "Inconnu";
   }

   private string MapStatutATItoBC14(int spId)
   {
      if (StatutATIversBC14.TryGetValue(spId, out var bc14Status))
         return bc14Status;
      return GetStatutATIDesignation(spId);
   }

   private bool StatutsAreDifferent(int spIdATI, string statusBC14)
   {
      if (string.IsNullOrEmpty(statusBC14)) return true;

      // Si le mapping est configuré, on l'utilise
      if (StatutATIversBC14.Count > 0)
      {
         var mappedStatus = MapStatutATItoBC14(spIdATI);
         return !string.Equals(mappedStatus, statusBC14, StringComparison.OrdinalIgnoreCase);
      }

      // Sinon, comparaison directe par désignation
      var designation = GetStatutATIDesignation(spIdATI);
      return !string.Equals(designation, statusBC14, StringComparison.OrdinalIgnoreCase);
   }

   private void Compare()
   {
      try
      {
         foreach (var p in ProjetsATI)
         {
            FicheBC14 f;
            bool isNotImpNumber = string.IsNullOrEmpty(p.ImpNumb);
            if (isNotImpNumber) f = ProjetsBC14.FirstOrDefault(x => x.CompNumber == p.CompNumber);
            else f = ProjetsBC14.FirstOrDefault(x => x.No == p.ImpNumb);

            if (f == null) continue;

            if (!StatutsAreDifferent(p.SpId, f.Status)) continue;

            var employe = employeList.FirstOrDefault(e => e.Id == p.RespAffaireId);
            string respATI = employe?.FullName ?? "Inconnu";

            bool isRespMismatch = false;
            if (string.Equals(f.Status, "Completed", StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrEmpty(f.Person_Responsible))
            {
               try
               {
                  isRespMismatch = int.Parse(f.Person_Responsible) != p.RespAffaireId
                     || (employe != null && !string.Equals(f.Responsable_Nom, employe.FullName, StringComparison.OrdinalIgnoreCase));
               }
               catch
               {
                  isRespMismatch = true;
               }
            }

            CompList.Add(new CompStatutBC14
            {
               No = f.No,
               ClientName = p.ClientName,
               Designation = p.Designation,
               Description = f.Description,
               StatutATI = GetStatutATIDesignation(p.SpId),
               StatutBC14 = f.Status ?? "Inconnu",
               RespATI = respATI,
               RespBC14 = f.Responsable_Nom ?? "",
               CompNumberATI = isNotImpNumber ? p.CompNumber : p.ImpNumb ?? p.CompNumber,
               CompNumberBC = f.CompNumber,
               IsResponsableMismatch = isRespMismatch,
            });
         }

         CompList = CompList.OrderBy(c => c.No).ToList();
      }
      catch (Exception e)
      {
         Console.WriteLine(e.Message);
         throw;
      }
   }

   private async void OpenInfo(CompStatutBC14 item)
   {
      var parameters = new Dictionary<string, object>();
      parameters.Add("Item", item);
      parameters.Add("ExcludedProp", new List<string> { "IsResponsableMismatch", "CompNumberATI", "CompNumberBC" });
      await modal.ShowAsync<ShowGeneric<CompStatutBC14>>(localizer["Details du projet"] + " " + item.No, parameters: parameters);
   }

   private void PageSizeChanged(int newSize)
   {
      pageSize = newSize;
   }
}
