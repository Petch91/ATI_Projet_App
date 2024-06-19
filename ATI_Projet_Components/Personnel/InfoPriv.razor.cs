using ATI_Projet_Models;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Net.Http.Json;
using Microsoft.Extensions.Localization;
using ATI_Projet_Cultures.Locales;
using ATI_Projet_Models.Events;

namespace ATI_Projet_Components.Personnel
{
   public partial class InfoPriv : ComponentBase
   {
      [Inject]
      private HttpClient HttpClient { get; set; }

      [Inject] private IStringLocalizer<PersonnelResource> localizer { get; set; }

      [Parameter]
      public EmployePrivate EmployePrivate { get; set; }

      private Adresse Adresse { get; set; }

      private Dictionary<string, string> Pays { get; set; }

      private int NewId;

      private string CodePays;

      protected async override void OnInitialized()
      {
         Pays = new Dictionary<string, string>();
         if (CultureInfo.CurrentCulture.Equals(new CultureInfo("fr-BE"))) Pays = await HttpClient.GetFromJsonAsync<Dictionary<string, string>>
                                                                                 ("https://www.iso-country-code.com/api/?lang=fr&output=json")
                                                                                 ?? new Dictionary<string, string>();

         else Pays = await HttpClient.GetFromJsonAsync<Dictionary<string, string>>("https://www.iso-country-code.com/api/?lang=en&output=json")
                                                                                ?? new Dictionary<string, string>();
         StateHasChanged();
      }

      //protected async override void OnAfterRender(bool firstRender)
      //{
      //   if (!firstRender)
      //   {
      //      Pays = new Dictionary<string, string>();
      //      if (CultureInfo.CurrentCulture.Equals(new CultureInfo("fr-BE"))) Pays = await HttpClient.GetFromJsonAsync<Dictionary<string, string>>
      //                                                                              ("https://www.iso-country-code.com/api/?lang=fr&output=json")
      //                                                                              ?? new Dictionary<string, string>();

      //      else Pays = await HttpClient.GetFromJsonAsync<Dictionary<string, string>>("https://www.iso-country-code.com/api/?lang=en&output=json")
      //                                                                              ?? new Dictionary<string, string>();
      //   }

      //}

      protected async override Task OnParametersSetAsync()
      {
         if (EmployePrivate != null)
         {
            Adresse = new Adresse();
            Adresse = await HttpClient.GetFromJsonAsync<Adresse>("Employe/adresse/" + EmployePrivate.AdresseId);
            Adresse.EmployeId = EmployePrivate.Id;
            CodePays = Adresse.Pays;
            StateHasChanged();
         }
      }
      public async Task EditPrivate(EditPrivateArgs employePrivate)
      {
         EmployePrivate = employePrivate.Employe;
         Adresse = employePrivate.Adresse;
         var reponse = HttpClient.PatchAsJsonAsync<Adresse>("Employe/updateAdresse", Adresse);
         EmployePrivate.AdresseId = await reponse.Result.Content.ReadFromJsonAsync<int>();
         await HttpClient.PatchAsJsonAsync<EmployePrivate>("Employe/updatePrivate", EmployePrivate);
         modal.HideAsync();
         CodePays = Adresse.Pays;
         StateHasChanged();
      }
      //public async Task EditAdresse(Adresse adresse)
      //{
      //   Adresse = adresse;
      //   var reponse = HttpClient.PatchAsJsonAsync<Adresse>("Employe/updateAdresse", adresse);
      //   NewId = await reponse.Result.Content.ReadFromJsonAsync<int>();
      //   //StateHasChanged();
      //}

      private Modal modal = default!;

      private async Task ShowEditPrivate()
      {
         var parameters = new Dictionary<string, object>();
         parameters.Add("EmployePrivate", EmployePrivate.Clone());
         parameters.Add("Adresse", Adresse.Clone());
         parameters.Add("Pays", Pays);
         parameters.Add("EditEmployePrivateEvent", EventCallback.Factory.Create<EditPrivateArgs>(this, EditPrivate));
         //parameters.Add("EditAdresseEvent", EventCallback.Factory.Create<Adresse>(this, EditAdresse));
         await modal.ShowAsync<EditPrivate>(title: localizer["Edition des infos privées"], parameters: parameters);
      }
   }
}