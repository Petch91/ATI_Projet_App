using ATI_Projet_Models;
using ATI_Projet_Models.Models.Forms;
using ATI_Projet_Models.Types;
using ATI_Projet_Models.VCA;
using ATI_Projets_Models;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http.Json;
using Microsoft.Extensions.Localization;
using System.Globalization;
using ATI_Projet_Cultures.Locales;
using Microsoft.Graph.Models.CallRecords;
using ATI_Projet_Models.Models.Societes.Clients;
using ATI_Projet_Models.Models.Projets;
using ATI_Projet_Models.Models;


namespace ATI_Projet_Components.Clients
{
   public partial class Fiche : ComponentBase, IDisposable
   {
      [Inject] private HttpClient httpClient { get; set; } = default!;

      [Inject] private IStringLocalizer<PersonnelResource> localizer { get; set; }

      [Parameter] public EventCallback<int> ClientChanged { get; set; }

      [Parameter] public int Id { get; set; }

      private ClientInfo ClientInfo { get; set; }
      private ClientSigna ClientSigna { get; set; }
      private Projet Projet{ get; set; }

      private IEnumerable<AxeMarche> axesMarche;
      private IEnumerable<DeptSimplifiedList> departements;

      private int choix = 0;

      private List<Email> emails;
      private List<Telephone> telephones;
      public List<ClientList> Liste { get; set; }


      private Offcanvas offcanvas = default!;

      protected async override Task OnInitializedAsync()
      {

         Liste = await httpClient.GetFromJsonAsync<List<ClientList>>("Client/list/") ?? new List<ClientList>();
         axesMarche = new List<AxeMarche>();
         axesMarche = await httpClient.GetFromJsonAsync<IEnumerable<AxeMarche>>("AxeMarche");
         departements = new List<DeptSimplifiedList>();
         departements = await httpClient.GetFromJsonAsync<IEnumerable<DeptSimplifiedList>>("departement/simplifiedList");
         if (Liste != null) StateHasChanged();
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
      }
      private async Task SelectChange(int id)
      {
         Id = id;

         await ChangeClient();
         await ClientChanged.InvokeAsync(Id);
         StateHasChanged();

      }

      private void choisir(int choice)
      {
         choix = choice;
         StateHasChanged();
      }

      public async Task ShowCanvas()
      {
         await offcanvas.ShowAsync();
      }
      public async Task OnHideOffcanvasClick()
      {
         await offcanvas.HideAsync();
      }
      public async Task TriggerEmail()
      {
         emails = await httpClient.GetFromJsonAsync<List<Email>>("Email/BySociete/" + ClientInfo.SocieteId);
         StateHasChanged();
      }

      public async Task TriggerTelephone()
      {
         telephones = await httpClient.GetFromJsonAsync<List<Telephone>>("Telephone/BySociete/" + ClientInfo.SocieteId);
         StateHasChanged();
      }

      public void ProfilChanged(EmployeProfil employeProfil)
      {
         EmployeProfil = employeProfil;
         Liste.First(e => e.Id == employeProfil.Id).Nom = employeProfil.Nom;
         Liste.First(e => e.Id == employeProfil.Id).Prenom = employeProfil.Prenom;
         StateHasChanged();
      }

      private async Task ChangeClient()
      {
         EmployeProfil = await httpClient.GetFromJsonAsync<EmployeProfil>("Employe/profil/" + Id) ?? new EmployeProfil();

         EmployePrivate = await httpClient.GetFromJsonAsync<EmployePrivate>("Employe/private/" + Id) ?? new EmployePrivate();
         PhotoPath = string.IsNullOrEmpty(EmployePrivate.Photo) ? "/images/Photo.jpg" : (Path.GetFullPath(EmployePrivate.Photo)).Replace("/app/wwwroot", "").Replace("\\", "/");
         SignaturePath = string.IsNullOrEmpty(EmployePrivate.Signature) ? "/images/signature.webp" : (Path.GetFullPath(EmployePrivate.Signature)).Replace("/app/wwwroot", "").Replace("\\", "/");

         EmployeProf = await httpClient.GetFromJsonAsync<EmployeProf>("Employe/prof/" + Id) ?? new EmployeProf();

         if (EmployeProfil != null)
         {
            emails = await httpClient.GetFromJsonAsync<List<Email>>("Employe/emailsByEmploye/" + EmployeProfil.PersonneId);

            telephones = await httpClient.GetFromJsonAsync<List<Telephone>>("Employe/telephonesByEmploye/" + EmployeProfil.PersonneId);
         }


      }


      private Modal modal { get; set; }
      private async Task OpenModalCreate()
      {
         var parameters = new Dictionary<string, object>();
         parameters.Add("CreateEvent", EventCallback.Factory.Create<PersonneForm>(this, AddEmploye));
         parameters.Add("Langues", Langues);
         parameters.Add("Nationalites", Nationalites);
         await modal.ShowAsync<CreatePersonne>(title: localizer["Ajouter un employé"], parameters: parameters);
      }

      private async Task AddEmploye(PersonneForm personne)
      {
         await httpClient.PostAsJsonAsync<PersonneForm>("Employe/", personne);
         await modal.HideAsync();
         Liste = await httpClient.GetFromJsonAsync<List<EmployeList>>("Employe/") ?? new List<EmployeList>();
         if (Liste != null) Id = Liste.Last().Id;
         await ChangeClient();
         StateHasChanged();
      }

      public void Dispose()
      {
         httpClient.Dispose();
         GC.SuppressFinalize(this);
      }
   }
}