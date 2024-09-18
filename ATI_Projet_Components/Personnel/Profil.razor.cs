using ATI_Projet_Models;
using ATI_Projet_Models.Models.Forms;
using ATI_Projet_Models.Types;
using ATI_Projet_Models.VCA;
using BlazorBootstrapPerso;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http.Json;
using Microsoft.Extensions.Localization;
using System.Globalization;
using ATI_Projet_Cultures.Locales;
using ATI_Projet_Tools.Services.Interfaces;
using ATI_Projet_Cultures.Tools;

namespace ATI_Projet_Components.Personnel
{
   enum GestionStock
   {
      NON = 0,
      Minimum = 1,
      Moyen = 5,
      Total = 9

   }
   public partial class Profil : IDisposable
   {
      [Inject] private ICommon common {  get; set; }
      [Inject] private IPersonnel personnel { get; set; }

      [Inject] private IStringLocalizer<PersonnelResource> localizer { get; set; }
      [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

      [Parameter]
      public EmployeProfil EmployeProfil { get; set; }
      [Parameter]
      public EventCallback<EmployeProfil> ProfilChanged { get; set; }


      private IEnumerable<Fonction> fonctions;
      private IEnumerable<FonctionVCA> fonctionsVCA;
      private IEnumerable<StatusVCA> statusVCA;
      private IEnumerable<TypeContrat> TypesContrat;
      private IEnumerable<TypeMO> TypesMO;
      private IEnumerable<DeptSimplifiedList> departements;




      protected async override Task OnInitializedAsync()
      {
         LanguageNotifier.SubscribeLanguageChange(this);
         fonctions = new List<Fonction>();
         fonctions = await personnel.GotFonctions();
         fonctionsVCA = new List<FonctionVCA>();
         fonctionsVCA = await personnel.GotFonctionsVCA();
         statusVCA = new List<StatusVCA>();
         statusVCA = await personnel.GotStatusVCA();
         TypesContrat = new List<TypeContrat>();
         TypesContrat = await personnel.GotTypesContrat();
         TypesMO = new List<TypeMO>();
         TypesMO = await personnel.GotTypesMO();
         departements = new List<DeptSimplifiedList>();
         departements = await common.GetDepts();
      }

      public void Dispose() => LanguageNotifier.UnsubscribeLanguageChange(this);

      public async void Edit(EmployeProfil employeProfil)
      {
         EmployeProfil = employeProfil;
         await personnel.EditProfil(employeProfil);
         modal.HideAsync();
         await ProfilChanged.InvokeAsync(EmployeProfil);
         StateHasChanged();
      }
      private Modal modal = default!;

      private async Task ShowEditProfil()
      {
         var parameters = new Dictionary<string, object>();
         parameters.Add("EmployeProfil", EmployeProfil.Clone());
         parameters.Add("fonctions", fonctions);
         parameters.Add("fonctionsVCA", fonctionsVCA);
         parameters.Add("statusVCA", statusVCA);
         parameters.Add("TypesMO", TypesMO);
         parameters.Add("TypesContrat", TypesContrat);
         parameters.Add("departements", departements);
         parameters.Add("EditProfilEvent", EventCallback.Factory.Create<EmployeProfil>(this, Edit));
         await modal.ShowAsync<EditProfil>(title: localizer["Edition du Profil"], parameters: parameters);
      }

   }
}