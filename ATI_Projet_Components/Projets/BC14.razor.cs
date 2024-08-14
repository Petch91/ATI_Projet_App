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
   private bool isPatchAll = false;

   private Grid<CompBC14> grid;
   private BlazorBootstrap.Modal modal;

   private BlazorBootstrap.Modal modalSelect;

   private int index = 0;

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
               Index = i,
            };
            CompList.Add(projet);
            i++;
         }

      }
      IQueryCompList = CompList.AsQueryable();

   }

   private async void PatchOne(CompBC14 fiche)
   {
      if (fiche.NewPerson > 0)
      {
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
            RemoveOne(fiche.Index);
         }
         if (!isPatchAll)
         {
            await grid.RefreshDataAsync();
            StateHasChanged();
         }
      }
   }

   private void PatchAll()
   {
      isPatchAll = true;
      foreach (var item in CompList)
      {
         PatchOne(item);
      }
      _navigationManager.Refresh(true);
   }

   private void RemoveOne(int index)
   {
      CompList.RemoveAt(index);
      StateHasChanged();
   }

   private async void OpenInfo(CompBC14 fiche)
   {
      var parameters = new Dictionary<string, object>();
      parameters.Add("Item", fiche);
      parameters.Add("ExcludedProp", new List<string> { "Index" });
      await modal.ShowAsync<ShowGeneric<CompBC14>>(localizer["Details du projet"] + " " + fiche.No, parameters: parameters);
   }

   private async void OpenSelect(string no, int i)
   {
      index = i;
      var parameters = new Dictionary<string, object>();
      parameters.Add("TItem", typeof(EmployeList));
      parameters.Add("TValue", typeof(int));
      parameters.Add("Data", employeList.Where(x => x.Actif));
      parameters.Add("TextField", (Func<EmployeList, string>)(item => item.FullName));
      parameters.Add("ValueField", (Func<EmployeList, int>)(item => item.Id));
      parameters.Add("DefaultItemValue", CompList.ElementAt(index).NewPerson);
      parameters.Add("DefaultItemText", CompList.ElementAt(index).NewPersonName);
      parameters.Add("SelectedValueChanged", EventCallback.Factory.Create<int>(this, SelectChange));
      await modalSelect.ShowAsync<SelectList<EmployeList, int>>(localizer["Modification"], parameters: parameters);
   }

   private void SelectChange(int i)
   {
      CompList.ElementAt(index).NewPerson = i;
      CompList.ElementAt(index).NewPersonName = employeList.First(e => e.Id == i).FullName;
      StateHasChanged();
      //await grid.RefreshDataAsync();
   }

   private async void OnHideModalClick()
   {
      await grid.RefreshDataAsync();
      StateHasChanged();
      await modalSelect.HideAsync();
   }
}