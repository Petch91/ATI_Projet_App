using ATI_Projet_Cultures.Locales;
using ATI_Projet_Models;
using ATI_Projet_Models.Models.Projets;
using ATI_Projet_Tools.Services.Interfaces;
using ATI_Projets_Models;
using BlazorBootstrap;
using Blazorise;
using Blazorise.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Threading;

namespace ATI_Projet_Components.Projets;

public partial class BC14 : ComponentBase
{
   [Inject] private IProjet _projet { get; set; }
   [Inject] private IPersonnel _personnel { get; set; }

   [Inject] private NavigationManager _navigationManager { get; set; }
   [Inject] private IStringLocalizer<PersonnelResource> localizer { get; set; }

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
   private BlazorBootstrap.Modal modal;

   private BlazorBootstrap.Modal modalSelect;

   private int index = 0;
   private string _no = "";

   private List<ToastMessage> messages = new List<ToastMessage>();
   private HashSet<CompBC14> selectedFiches = new HashSet<CompBC14>();

   protected async override Task OnInitializedAsync()
   {
      ProjetsATI = await _projet.GotProjetsBc14();
      ProjetsBC14 = await _projet.GotFichesBc14();
      employeList = await _personnel.GotPersonnelList();
      ressourceList = await _projet.GotRessourcesBc14();

      Compare();

      if (CompList.Count > 0)
      {
         isOK = true;
      }

      //StateHasChanged();

   }

   private void Compare()
   {
      int i = 0;
      foreach (var p in ProjetsATI)
      {
         FicheBC14 f;
         bool isNotImpNumber = string.IsNullOrEmpty(p.ImpNumb);
         if (isNotImpNumber) f = ProjetsBC14.FirstOrDefault(x => x.CompNumber == p.CompNumber);
         else f = ProjetsBC14.FirstOrDefault(x => x.No == p.ImpNumb);
         var employe = employeList.First(e => e.Id == p.RespFacturationId);

         if (f != null && int.Parse(f.Person_Responsible) != p.RespFacturationId && f.Responsable_Nom.ToLower() != employe.FullName.ToLower())
         {
            CompBC14 projet = new CompBC14
            {
               No = f.No,
               RespFacturationId = p.RespFacturationId,
               RespATI = employe.FullName,
               Person_Responsible = f.Person_Responsible,
               RespBC14 = f.Responsable_Nom,
               ClientName = p.ClientName,
               CompNumberATI = isNotImpNumber ? p.CompNumber : p.ImpNumb ?? p.CompNumber,
               CompNumberBC = f.CompNumber,
               Description = f.Description,
               Designation = p.Designation,
               NewPerson = employe.Actif ? p.RespFacturationId : 0,
               NewPersonName = employe.Actif ? employe.FullName : "NA",
            };
            CompList.Add(projet);

         }

      }
      CompList = CompList.OrderBy(comp => comp.No).ToList();
      IQueryCompList = CompList.AsQueryable();

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
      if(selectedFiches.Count() > 1)
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
         if (compteur >= grid.PageSize) await grid.ResetPageNumber();
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
      if (compteur >= grid.PageSize) await grid.ResetPageNumber();
   }

   private async void OpenInfo(CompBC14 fiche)
   {
      var parameters = new Dictionary<string, object>();
      parameters.Add("Item", fiche);
      parameters.Add("ExcludedProp", new List<string> { "Index" });
      await modal.ShowAsync<ShowGeneric<CompBC14>>(localizer["Details du projet"] + " " + fiche.No, parameters: parameters);
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
}