using ATI_Projet_Cultures.Locales;
using ATI_Projet_Cultures.Tools;
using ATI_Projet_Models;
using ATI_Projet_Models.Models.Societes.Clients;
using ATI_Projet_Tools.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace ATI_Projet_Components.Clients;

public partial class ClientSignaCpt : ComponentBase, IDisposable
{
   [Inject] private IStringLocalizer<PersonnelResource> localizer { get; set; }
   [Inject] private IStringLocalizer<PaysResource> paysLocalizer { get; set; }
   [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

   [Inject] private ICommon common { get; set; }
   [Inject] private ISociete societe { get; set; }

   [Parameter] public ClientSigna client { get; set; }
   [Parameter] public List<Email> emails { get; set; }
   [Parameter] public List<Telephone> telephones { get; set; }

   private Adresse Adresse { get; set; }

   private string CodePays = "be";
   private Dictionary<string, string> Pays { get; set; }

   protected override void OnInitialized() => LanguageNotifier.SubscribeLanguageChange(this);
   public void Dispose() => LanguageNotifier.UnsubscribeLanguageChange(this);

   protected async override Task OnParametersSetAsync()
   {
      Adresse = new Adresse();
      if (client != null)
      {
         Adresse = await common.GetAdresse(client.AdresseId);
         CodePays = Adresse.Pays;

         StateHasChanged();
      }
      client = new ClientSigna();
   }
}