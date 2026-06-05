using ATI_Projet_Cultures.Locales;
using ATI_Projet_Cultures.Tools;
using ATI_Projet_Models.Models.Projets;
using ATI_Projet_Tools.Services.Interfaces;
using ATI_Projets_Models;
using BlazorBootstrapPerso;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Diagnostics;

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
   private bool isError = false;
   private string errorMessage = "";
   private int pageSize = 15;

   // Mapping: ATI StatutProjet.Id → liste de Status_EEB BC14 acceptables
   private static readonly Dictionary<int, List<string>> MappingATIversEEB = new()
   {
      { 0,   new List<string>() },
      { 10,  new List<string> { "DEVIS EN COURS" } },
      { 20,  new List<string> { "DEVIS TRANSFERE" } },
      { 30,  new List<string> { "ANNULE" } },
      { 40,  new List<string> { "DEVIS PERDU", "DEVIS OBSOLETE" } },
      { 50,  new List<string> { "COMMANDE - EXECUTION" } },
      { 60,  new List<string> { "COMMANDE - EXECUTION", "101 – ACTIF", "INTERNE" } },
      { 70,  new List<string> { "CHANTIER CLOTURE", "LEVEE DE CAUTIONNEME" } },
      { 80,  new List<string> { "203 - VENDU", "202 - VENDU (NON CLO)", "CHANTIER CLOTURE" } },
      { 90,  new List<string> { "EN GARANTIE", "LEVEE DE CAUTIONNEME" } },
      { 100, new List<string> { "CHANTIER CLOTURE", "203 - VENDU" } },
      { 200, new List<string> { "R&D", "RP EN ATTENTE DE RD" } },
   };

   protected async override Task OnInitializedAsync()
   {
      LanguageNotifier.SubscribeLanguageChange(this);
      LanguageNotifier.SubscribeLanguageChange(grid);

      try
      {
         var swTotal = Stopwatch.StartNew();

         // Lancement des 4 appels réseau en parallèle (indépendants)
         var swFetch = Stopwatch.StartNew();
         var tProjetsATI = _projet.GotProjetsCompStatut();
         var tProjetsBC14 = _projet.GotAllFichesBc14();
         var tEmploye = _personnel.GotPersonnelList();
         var tStatuts = _projet.GotStatuts();

         await Task.WhenAll(tProjetsATI, tProjetsBC14, tEmploye, tStatuts);

         ProjetsATI = tProjetsATI.Result;
         ProjetsBC14 = tProjetsBC14.Result;
         employeList = tEmploye.Result;
         statutsATI = tStatuts.Result;
         swFetch.Stop();
         Console.WriteLine($"[CompStatut] Fetch réseau: {swFetch.ElapsedMilliseconds} ms " +
            $"(ATI={ProjetsATI.Count()}, BC14={ProjetsBC14.Count()}, Employes={employeList.Count()}, Statuts={statutsATI.Count()})");

         var swCompare = Stopwatch.StartNew();
         Compare();
         swCompare.Stop();
         Console.WriteLine($"[CompStatut] Compare(): {swCompare.ElapsedMilliseconds} ms ({CompList.Count} lignes)");

         swTotal.Stop();
         Console.WriteLine($"[CompStatut] TOTAL: {swTotal.ElapsedMilliseconds} ms");
      }
      catch (HttpRequestException ex)
      {
         isError = true;
         errorMessage = "Impossible de se connecter au serveur BC14";
         Console.WriteLine(ex.Message);
      }
      catch (Exception ex)
      {
         isError = true;
         errorMessage = "Erreur lors du chargement des données: " + ex.Message;
         Console.WriteLine(ex.Message);
      }

      isOK = true;
   }

   public void Dispose()
   {
      LanguageNotifier.UnsubscribeLanguageChange(this);
      LanguageNotifier.UnsubscribeLanguageChange(grid);
   }

   // Dictionnaires de lookup construits une fois avant la comparaison
   private Dictionary<int, string> _statutsDict;
   private Dictionary<int, string> _employesDict;
   private Dictionary<string, FicheBC14> _bc14ByNo;
   private Dictionary<string, FicheBC14> _bc14ByCompNumber;

   private string GetStatutATIDesignation(int spId)
   {
      return _statutsDict.TryGetValue(spId, out var d) ? d : "Inconnu";
   }

   private bool StatutsAreDifferent(int spIdATI, string statusEEB)
   {
      if (string.IsNullOrEmpty(statusEEB)) return true;

      if (MappingATIversEEB.TryGetValue(spIdATI, out var acceptableStatuts))
      {
         // Si la liste est vide (code 0), on ne peut pas comparer
         if (acceptableStatuts.Count == 0) return true;
         return !acceptableStatuts.Any(s => string.Equals(s, statusEEB, StringComparison.OrdinalIgnoreCase));
      }

      // Code ATI inconnu → mismatch
      return true;
   }

   private void Compare()
   {
      try
      {
         // Pré-construction des index O(1) (évite les scans linéaires dans la boucle)
         _statutsDict = statutsATI
            .GroupBy(s => s.Id)
            .ToDictionary(g => g.Key, g => g.First().Designation);

         _employesDict = employeList
            .GroupBy(e => e.Id)
            .ToDictionary(g => g.Key, g => g.First().FullName);

         _bc14ByNo = new Dictionary<string, FicheBC14>();
         _bc14ByCompNumber = new Dictionary<string, FicheBC14>();
         foreach (var f in ProjetsBC14)
         {
            if (!string.IsNullOrEmpty(f.No) && !_bc14ByNo.ContainsKey(f.No))
               _bc14ByNo[f.No] = f;

            var comp = f.CompNumber;
            if (!string.IsNullOrEmpty(comp) && !_bc14ByCompNumber.ContainsKey(comp))
               _bc14ByCompNumber[comp] = f;
         }

         foreach (var p in ProjetsATI)
         {
            FicheBC14 f;
            bool isNotImpNumber = string.IsNullOrEmpty(p.ImpNumb);
            if (isNotImpNumber) _bc14ByCompNumber.TryGetValue(p.CompNumber, out f);
            else _bc14ByNo.TryGetValue(p.ImpNumb, out f);

            if (f == null) continue;

            bool isMismatch = StatutsAreDifferent(p.SpId, f.Status_EEB);
            if (!isMismatch) continue;

            string respATI = _employesDict.TryGetValue(p.RespAffaireId, out var nom) ? nom : "Inconnu";

            CompList.Add(new CompStatutBC14
            {
               No = f.No,
               ClientName = p.ClientName,
               Designation = p.Designation,
               Description = f.Description,
               StatutATI = GetStatutATIDesignation(p.SpId),
               StatutEEB = f.Status_EEB ?? "",
               StatutBC14 = f.Status ?? "",
               RespATI = respATI,
               RespBC14 = f.Responsable_Nom ?? "",
               CompNumberATI = isNotImpNumber ? p.CompNumber : p.ImpNumb ?? p.CompNumber,
               CompNumberBC = f.CompNumber,
               IsStatutMismatch = isMismatch,
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
      parameters.Add("ExcludedProp", new List<string> { "IsStatutMismatch", "CompNumberATI", "CompNumberBC" });
      await modal.ShowAsync<ShowGeneric<CompStatutBC14>>(localizer["Details du projet"] + " " + item.No, parameters: parameters);
   }

   private void PageSizeChanged(int newSize)
   {
      pageSize = newSize;
   }
}
