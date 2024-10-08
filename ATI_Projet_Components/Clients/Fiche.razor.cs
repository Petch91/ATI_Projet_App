using ATI_Projet_Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ATI_Projet_Cultures.Locales;
using ATI_Projet_Models.Models.Societes.Clients;
using ATI_Projet_Models.Models.Projets;
using ATI_Projet_Models.Models;
using ATI_Projet_Tools.Services.Interfaces;
using ATI_Projet_Cultures.Tools;
using BlazorBootstrapPerso;


namespace ATI_Projet_Components.Clients
{
   public partial class Fiche : ComponentBase, IDisposable
   {
      [Inject] private ICommon common { get; set; }
      [Inject] private NavigationManager navigationManager { get; set; }

      [Inject] private ISociete societe { get; set; }

      [Inject] private IStringLocalizer<PersonnelResource> localizer { get; set; }
      [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

      [Parameter] public EventCallback<int> ClientChanged { get; set; }

      [Parameter] public int Id { get; set; }

      private ClientInfo ClientInfo { get; set; }
      private ClientSigna ClientSigna { get; set; }
      private Projet Projet { get; set; }

      private IEnumerable<AxeMarche> axesMarche;
      private IEnumerable<DeptSimplifiedList> departements;

      private List<Email> emails;
      private List<Telephone> telephones;

      private int choix = 0;

      public List<ClientList> Liste { get; set; }

      private Offcanvas offcanvas = default!;

      private Tab tabs = default!;


      protected async override Task OnInitializedAsync()
      {
         LanguageNotifier.SubscribeLanguageChange(this);
         LanguageNotifier.SubscribeLanguageChange(tabs);
         var res = await societe.GotClientList();
         Liste = res.ToList();
         axesMarche = new List<AxeMarche>();
         axesMarche = await societe.GotAxeMarche();
         departements = new List<DeptSimplifiedList>();
         departements = await common.GetDepts();
         StateHasChanged();
      }
      public void Dispose()
      {
         LanguageNotifier.UnsubscribeLanguageChange(this);
         LanguageNotifier.UnsubscribeLanguageChange(tabs);
      }

      protected async override Task OnParametersSetAsync()
      {
         if (Id <= 0)
         {
            if (Liste != null)
            {
               Id = Liste.First().Id;
               await ChangeClient();
            }
         }
         else
            await ChangeClient();
         StateHasChanged();
      }
      private async Task SelectChange(int id)
      {
         if (Liste.Select(x => x.Id).Contains(id))
         {
            Id = id;

            await ChangeClient();
            await ClientChanged.InvokeAsync(Id);
            StateHasChanged();
            await offcanvas.HideAsync();
         }

      }

      private void choisir(int choice)
      {
         choix = choice;
         StateHasChanged();
      }


      public void InfoChanged(ClientInfo client)
      {
         ClientInfo = client;
         Liste.First(c => c.Id == client.Id).Name = client.Name;
         StateHasChanged();
      }

      private async Task ChangeClient()
      {
         ClientInfo = await societe.GotClientInfo(Id);

         ClientSigna = await societe.GotClientSigna(Id);

         var e = await societe.GotEmails(ClientSigna.SocieteId);
         //emails = new List<Email>();
         emails = e.ToList();

         var t = await societe.GotPhones(ClientSigna.SocieteId);
         //telephones = new List<Telephone>();
         telephones = t.ToList();


      }

      public async Task ShowCanvas()
      {
         await offcanvas.ShowAsync();
      }
      public async Task OnHideOffcanvasClick()
      {
         await offcanvas.HideAsync();
      }

      public void GoBC14()
      {
         navigationManager.NavigateTo("/BC14");
      }

   }
}