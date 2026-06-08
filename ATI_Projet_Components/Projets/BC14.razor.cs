using ATI_Projet_Cultures.Locales;
using ATI_Projet_Cultures.Tools;
using ATI_Projet_Models;
using ATI_Projet_Models.Models.Projets;
using ATI_Projet_Tools.Services.Interfaces;
using ATI_Projets_Models;
using BlazorBootstrapPerso;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.Extensions.Localization;
using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Functions.Log10;
using Microsoft.JSInterop;
using Serilog;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Threading;

namespace ATI_Projet_Components.Projets;

public partial class BC14 : ComponentBase, IDisposable
{
   [Inject] private IProjet _projet { get; set; }
   [Inject] private IPersonnel _personnel { get; set; }

   [Inject] private NavigationManager _navigationManager { get; set; }
   [Inject] private IStringLocalizer<PersonnelResource> localizer { get; set; }
   [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

   [Inject] public IJSRuntime JS { get; set; } = default!;

   private IEnumerable<ProjetBC14> ProjetsATI { get; set; }
   private IEnumerable<FicheBC14> ProjetsBC14 { get; set; }
   private IEnumerable<EmployeList> employeList { get; set; }
   private IEnumerable<RessourceBC14> ressourceList { get; set; }


   private List<CompBC14> CompList = new List<CompBC14>();
   private IQueryable<CompBC14> IQueryCompList;
   private PaginationState pagination = new PaginationState { ItemsPerPage = 25 };

   private bool isOK = false;
   private bool isPatchSelected = false;
   private bool isPatch = false;

   private Grid<CompBC14> grid = new Grid<CompBC14>();
   private BlazorBootstrapPerso.Modal modal;

   private BlazorBootstrapPerso.Modal modalSelect;

   private int index = 0;
   private int pageSize = 15;
   private string _no = "";

   private List<ToastMessage> messages = new List<ToastMessage>();
   private HashSet<CompBC14> selectedFiches = new HashSet<CompBC14>();

   protected async override Task OnInitializedAsync()
   {
      LanguageNotifier.SubscribeLanguageChange(this);
      LanguageNotifier.SubscribeLanguageChange(grid);

      var swTotal = Stopwatch.StartNew();

      async Task<T> Timed<T>(string nom, Task<T> task)
      {
         var sw = Stopwatch.StartNew();
         var res = await task;
         sw.Stop();
         Console.WriteLine($"[BC14]   - {nom}: {sw.ElapsedMilliseconds} ms");
         return res;
      }

      // Les 4 appels en parallèle (indépendants)
      var swFetch = Stopwatch.StartNew();
      var tATI = Timed("ATI projets (api)", _projet.GotProjetsBc14());
      var tBC14 = Timed("BC14 fiches (sync)", _projet.GotFichesBc14());
      var tEmploye = Timed("Employés (api)", _personnel.GotPersonnelList());
      var tRes = Timed("Ressources (sync)", _projet.GotRessourcesBc14());

      await Task.WhenAll(tATI, tBC14, tEmploye, tRes);

      ProjetsATI = tATI.Result;
      ProjetsBC14 = tBC14.Result;
      employeList = tEmploye.Result;
      ressourceList = tRes.Result;
      swFetch.Stop();
      Console.WriteLine($"[BC14] Fetch réseau: {swFetch.ElapsedMilliseconds} ms " +
         $"(ATI={ProjetsATI.Count()}, BC14={ProjetsBC14.Count()}, Employes={employeList.Count()}, Res={ressourceList.Count()})");

      var swCompare = Stopwatch.StartNew();
      Compare();
      swCompare.Stop();
      Console.WriteLine($"[BC14] Compare(): {swCompare.ElapsedMilliseconds} ms ({CompList.Count} lignes)");

      swTotal.Stop();
      Console.WriteLine($"[BC14] TOTAL: {swTotal.ElapsedMilliseconds} ms");

      if (CompList.Count > 0)
      {
         isOK = true;
      }
   }

   public void Dispose() { LanguageNotifier.UnsubscribeLanguageChange(this); LanguageNotifier.UnsubscribeLanguageChange(grid); }

   private void Compare()
   {
        try
        {
            // Index O(1) (évite les FirstOrDefault/First en scan linéaire dans la boucle)
            var bc14ByNo = new Dictionary<string, FicheBC14>();
            var bc14ByCompNumber = new Dictionary<string, FicheBC14>();
            foreach (var x in ProjetsBC14)
            {
                if (!string.IsNullOrEmpty(x.No) && !bc14ByNo.ContainsKey(x.No))
                    bc14ByNo[x.No] = x;
                var comp = x.CompNumber;
                if (!string.IsNullOrEmpty(comp) && !bc14ByCompNumber.ContainsKey(comp))
                    bc14ByCompNumber[comp] = x;
            }
            var employesById = new Dictionary<int, EmployeList>();
            foreach (var e in employeList)
                if (!employesById.ContainsKey(e.Id))
                    employesById[e.Id] = e;

            foreach (var p in ProjetsATI)
            {
                FicheBC14 f;
                bool isNotImpNumber = string.IsNullOrEmpty(p.ImpNumb);
                if (isNotImpNumber) bc14ByCompNumber.TryGetValue(p.CompNumber, out f);
                else bc14ByNo.TryGetValue(p.ImpNumb, out f);

                if (f == null) continue;

                if (!employesById.TryGetValue(p.RespAffaireId, out var employe))
                    continue;   // pas d'employé correspondant → on saute (évite le crash de .First())

                if (!string.IsNullOrEmpty(f.Person_Responsible) && int.Parse(f.Person_Responsible) != p.RespAffaireId && !NomsEquivalents(f.Responsable_Nom, employe.FullName))
                {
                    CompBC14 projet = new CompBC14
                    {
                        No = f.No,
                        RespAffaireId = p.RespAffaireId,
                        RespATI = employe.FullName,
                        Person_Responsible = f.Person_Responsible,
                        RespBC14 = f.Responsable_Nom,
                        ClientName = p.ClientName,
                        CompNumberATI = isNotImpNumber ? p.CompNumber : p.ImpNumb ?? p.CompNumber,
                        CompNumberBC = f.CompNumber,
                        Description = f.Description,
                        Designation = p.Designation,
                        NewPerson = employe.Actif ? p.RespAffaireId : 0,
                        NewPersonName = employe.Actif ? employe.FullName : "NA",
                    };
                    CompList.Add(projet);

                }
                else if (string.IsNullOrEmpty(f.Person_Responsible))
                {
                    Console.WriteLine($"ce client {f.Description} n'a pas de responsable");
                }

            }
            CompList = CompList.OrderBy(comp => comp.No).ToList();
            IQueryCompList = CompList.AsQueryable();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

   }

   // Compare deux noms en ignorant la casse ET les accents (é, ë, è… = e).
   // Évite de signaler le même responsable orthographié différemment entre ATI et BC14
   // (ex : "DENOËL Jean-Yves" == "DENOEL Jean-Yves").
   private static bool NomsEquivalents(string a, string b)
   {
      if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return false;
      return string.Compare(a.Trim(), b.Trim(),
         CultureInfo.InvariantCulture,
         CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0;
   }

   private async Task<bool> PatchOne(CompBC14 fiche)
   {

      if (fiche.NewPerson > 0)
      {
         isPatch = true;
         if (!ressourceList.Select(x => x.No).Contains(fiche.NewPerson.ToString()))
         {
            try
            {
               fiche.NewPerson = int.Parse(ressourceList.FirstOrDefault(x => x.Name.ToLower() == employeList.First(e => e.Id == fiche.NewPerson).FullName.ToLower()).No ?? "57");
            }
            catch
            {
               fiche.NewPerson = 57;
            }
         }
         if (await _projet.PatchFicheBC14(fiche.No, fiche.NewPerson.ToString()))
         {
            await RemoveOne(fiche.No);
            if (!isPatchSelected)
            {
               messages.Add(new ToastMessage { Type = ToastType.Success, Message = $"Fiche No {fiche.No} est corrigée" });

            }
            isPatch = false;
            StateHasChanged();
            return true;
         }
         else
         {
            messages.Add(new ToastMessage { Type = ToastType.Danger, Message = $"Erreur pour la correction de la fiche No {fiche.No}" });
            isPatch = false;
            StateHasChanged();
            return false;
         }

      }
      return false;
   }

   private async void PatchSelected()
   {
      if (selectedFiches.Count() > 1)
      {
         int compteur = 0;
         isPatchSelected = true;
         foreach (var item in selectedFiches)
         {
            if (await PatchOne(item)) compteur++;
         }
         isPatchSelected = false;
         messages.Add(new ToastMessage { Type = ToastType.Success, Message = $"{compteur} sur {selectedFiches.Count()} fiches ont été corrigées" });
         StateHasChanged();
         selectedFiches.Clear();
         await grid.RefreshDataAsync();
         if (compteur >= grid.PageSize)
            await grid.ResetPageNumber();
         StateHasChanged();
      }
   }

   private async Task RemoveOne(string no)
   {
      CompList.Remove(CompList.First(x => x.No == no));
      if (!isPatch) { messages.Add(new ToastMessage { Type = ToastType.Warning, Message = $"Fiche No {no} a été supprimée de la liste\nActualiser la page pour recuperer la liste complète" }); }
      if (!isPatchSelected) { await grid.RefreshDataAsync(); StateHasChanged(); }
   }

   private async void RemoveSelected()
   {
      int compteur = 0;
      foreach (var item in selectedFiches)
      {
         CompList.Remove(CompList.First(x => x.No == item.No));
         compteur++;
      }
      messages.Add(new ToastMessage { Type = ToastType.Warning, Message = $"{compteur} sur {selectedFiches.Count()} fiches ont été supprimées de la liste\nActualiser la page pour recuperer la liste complète" });
      StateHasChanged();
      selectedFiches.Clear();
      await grid.RefreshDataAsync();
      if (compteur >= grid.PageSize)
         await grid.ResetPageNumber();
   }

   private async void OpenInfo(CompBC14 fiche)
   {
      var parameters = new Dictionary<string, object>();
      parameters.Add("Item", fiche);
      await modal.ShowAsync<DetailsBC14>(localizer["Details du projet"] + " " + fiche.No, parameters: parameters);
   }

   private async void OpenAide()
   {
      await modal.ShowAsync<AideBC14>("Aide — Comparaison BC14");
   }

   //private async void OpenSelect(string no, int i)
   //{
   //   _no = no;
   //   var parameters = new Dictionary<string, object>();
   //   parameters.Add("TItem", typeof(EmployeList));
   //   parameters.Add("TValue", typeof(int));
   //   parameters.Add("Data", employeList.Where(x => x.Actif));
   //   parameters.Add("TextField", (Func<EmployeList, string>)(item => item.FullName));
   //   parameters.Add("ValueField", (Func<EmployeList, int>)(item => item.Id));
   //   parameters.Add("DefaultItemValue", CompList.First(x => x.No == no).NewPerson);
   //   parameters.Add("DefaultItemText", CompList.First(x => x.No == no).NewPersonName);
   //   parameters.Add("SelectedValueChanged", EventCallback.Factory.Create<int>(this, SelectChange));
   //   await modalSelect.ShowAsync<SelectList<EmployeList, int>>(localizer["Modification"], parameters: parameters);
   //}

   //private void SelectChange(int i)
   //{
   //   CompList.First(x => x.No == _no).NewPerson = i;
   //   CompList.First(x => x.No == _no).NewPersonName = employeList.First(e => e.Id == i).FullName;
   //   StateHasChanged();
   //   //await grid.RefreshDataAsync();
   //}

   private async void OnHideModalClick()
   {
      await grid.RefreshDataAsync();
      StateHasChanged();
      await modalSelect.HideAsync();
   }
   private Task OnSelectedItemsChanged(HashSet<CompBC14> fiches)
   {
      selectedFiches = fiches is not null && fiches.Any() ? fiches : new();
      StateHasChanged();
      return Task.CompletedTask;
   }
   private bool DisableAllRowsSelectionHandler(IEnumerable<CompBC14> fiches)
   {
      return fiches.Any(x => x.NewPerson == 0);
   }
   private bool DisableRowSelectionHandler(CompBC14 fiche)
   {
      return fiche.NewPerson == 0;
   }
   private void PageSizeChanged(int newSize)
   {
      pageSize = newSize;
   }
}