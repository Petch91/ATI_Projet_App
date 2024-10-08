using ATI_Projet_Models;
using BlazorBootstrapPerso;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Net.Http.Json;
using Microsoft.Extensions.Localization;
using ATI_Projet_Cultures.Locales;
using ATI_Projet_Models.Events;
using ATI_Projet_Tools.Services.Interfaces;
using ATI_Projet_Cultures.Tools;

namespace ATI_Projet_Components.Personnel
{
   public partial class InfoPriv : ComponentBase, IDisposable
   {
      [Inject] private IPersonnel personnel { get; set; }

      [Inject] private IStringLocalizer<PersonnelResource> localizer { get; set; }
      [Inject] private IStringLocalizer<LangueResource> langLocalizer { get; set; }
      [Inject] private IStringLocalizer<NationaliteResource> natLocalizer { get; set; }
      [Inject] private IStringLocalizer<PaysResource> paysLocalizer { get; set; }
      [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }


      [Parameter]
      public EmployePrivate EmployePrivate { get; set; }

      private Adresse Adresse { get; set; }

      private string CodePays;
      private Dictionary<string, string> Pays { get; set; }
      private Dictionary<string, string> Langues { get; set; }
      private Dictionary<string, string> Nationalites { get; set; }

      private int NewId;


      protected override void OnInitialized()
      {
         LanguageNotifier.SubscribeLanguageChange(this);
         var dico = langLocalizer.GetAllStrings().ToDictionary(x => x.Name, x => x.Value);
         var valeurs = dico.Values.ToList();
         valeurs.Sort();
         Langues = new Dictionary<string, string>();
         foreach (var value in valeurs)
         {
            foreach (var kvp in dico)
            {
               if (kvp.Value == value)
               {
                  Langues.Add(kvp.Key, value);
                  break;
               }
            }
         }
         dico = natLocalizer.GetAllStrings().ToDictionary(x => x.Name, x => x.Value);
         valeurs = dico.Values.ToList();
         valeurs.Sort();
         Nationalites = new Dictionary<string, string>();
         foreach (var value in valeurs)
         {
            foreach (var kvp in dico)
            {
               if (kvp.Value == value)
               {
                  Nationalites.Add(kvp.Key, value);
                  break;
               }
            }
         }

         dico = paysLocalizer.GetAllStrings().ToDictionary(x => x.Name, x => x.Value);
         valeurs = dico.Values.ToList();
         valeurs.Sort();
         Pays = new Dictionary<string, string>();
         foreach (var value in valeurs)
         {
            foreach (var kvp in dico)
            {
               if (kvp.Value == value)
               {
                  Pays.Add(kvp.Key, value);
                  break;
               }
            }
         }
         StateHasChanged();
      }

      public void Dispose() => LanguageNotifier.UnsubscribeLanguageChange(this);

      protected async override Task OnParametersSetAsync()
      {
         if (EmployePrivate != null)
         {

            Adresse = new Adresse();
            Adresse = await personnel.GotAdresse((int)EmployePrivate.AdresseId!);
            Adresse.EmployeId = EmployePrivate.Id;
            CodePays = Adresse.Pays;
            StateHasChanged();
         }
      }
      public async Task EditPrivate(EditPrivateArgs employePrivate)
      {
         EmployePrivate = employePrivate.Employe;
         Adresse = employePrivate.Adresse;
         Adresse.Pays += Pays.GetValueOrDefault(Adresse.Pays);
         EmployePrivate.AdresseId = await personnel.EditAdresse(Adresse);
         Adresse.Pays = Adresse.Pays.Substring(2);
         await personnel.EditPrivate(EmployePrivate);
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
         parameters.Add("Langues", Langues);
         parameters.Add("Nationalites", Nationalites);
         parameters.Add("EditEmployePrivateEvent", EventCallback.Factory.Create<EditPrivateArgs>(this, EditPrivate));
         //parameters.Add("EditAdresseEvent", EventCallback.Factory.Create<Adresse>(this, EditAdresse));
         await modal.ShowAsync<EditPrivate>(title: localizer["Edition des infos priv�es"], parameters: parameters);
      }
   }
}