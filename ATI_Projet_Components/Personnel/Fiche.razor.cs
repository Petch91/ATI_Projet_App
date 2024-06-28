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


namespace ATI_Projet_Components.Personnel
{
   public partial class Fiche : ComponentBase, IDisposable
   {
      [Inject] private HttpClient httpClient { get; set; } = default!;

      [Inject] private IStringLocalizer<PersonnelResource> localizer { get; set; }
      [Inject] private IStringLocalizer<LangueResource> langLocalizer { get; set; }
      [Inject] private IStringLocalizer<NationaliteResource> natLocalizer { get; set; }

      [Parameter] public EventCallback<int> EditPhoto { get; set; }
      [Parameter] public EventCallback<int> EditSignature { get; set; }
      [Parameter] public EventCallback<int> EmployeChanged { get; set; }

      [Parameter] public int Id { get; set; }

      private EmployeProfil EmployeProfil { get; set; }
      private EmployePrivate EmployePrivate { get; set; }
      private EmployeProf EmployeProf { get; set; }

      private IEnumerable<Fonction> fonctions;
      private IEnumerable<DeptSimplifiedList> departements;

      private int choix = 0;

      private List<Email> emails;
      private List<Telephone> telephones;
      public List<EmployeList> Liste { get; set; }

      private string PhotoPath;
      private string SignaturePath;

      private Dictionary<string, string> Langues { get; set; }
      private Dictionary<string, string> Nationalites { get; set; }

      private Offcanvas offcanvas = default!;

      CancellationTokenSource cts = new CancellationTokenSource();

      // Créer un jeton d'annulation à partir du CancellationTokenSource
      CancellationToken token = default!;

      // Lancer une tâche asynchrone qui utilise le jeton d'annulation
      Task task = default!;
      protected async override Task OnInitializedAsync()
      {
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

         Liste = await httpClient.GetFromJsonAsync<List<EmployeList>>("Employe/") ?? new List<EmployeList>();
         fonctions = new List<Fonction>();
         fonctions = await httpClient.GetFromJsonAsync<IEnumerable<Fonction>>("Fonction");
         departements = new List<DeptSimplifiedList>();
         departements = await httpClient.GetFromJsonAsync<IEnumerable<DeptSimplifiedList>>("departement/simplifiedList");
         token = cts.Token;
         task = Task.Run(async () =>
         {
            await Task.Delay(300, token);
         }, token);
         if (Liste != null) StateHasChanged();
      }

      protected async override Task OnParametersSetAsync()
      {
         if (Id <= 0)
         {
            if (Liste != null)
            { 
               Id = Liste.First().Id;
               await ChangeEmploye();
            }
         }
         else
         await ChangeEmploye();
      }
      private async Task SelectChange(int id)
      {
         Id = id;
         await task;
         if (!task.IsCanceled)
         {

            await ChangeEmploye();
            await EmployeChanged.InvokeAsync(Id);
            StateHasChanged();
         }

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
         emails = await httpClient.GetFromJsonAsync<List<Email>>("Employe/emailsByEmploye/" + EmployeProfil.PersonneId);
         StateHasChanged();
      }

      public async Task TriggerTelephone()
      {
         telephones = await httpClient.GetFromJsonAsync<List<Telephone>>("Employe/telephonesByEmploye/" + EmployeProfil.PersonneId);
         StateHasChanged();
      }

      public void ProfilChanged(EmployeProfil employeProfil)
      {
         EmployeProfil = employeProfil;
         Liste.First(e => e.Id == employeProfil.Id).Nom = employeProfil.Nom;
         Liste.First(e => e.Id == employeProfil.Id).Prenom = employeProfil.Prenom;
         StateHasChanged();
      }

      private async Task ChangeEmploye()
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

      private async Task ChangePhoto()
      {
         await EditPhoto.InvokeAsync(Id);

      }

      private async Task ChangeSignature()
      {
         await EditSignature.InvokeAsync(Id);
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
         await ChangeEmploye();
         StateHasChanged();
      }

      private string UsaSize(int size)
      {
         double pouces = size * 0.393701;
         int feets = (int)pouces / 12;
         int reste = (int)pouces % 12;
         return $"{feets}ft {reste}\"";
      }

      public void Dispose()
      {
         httpClient.Dispose();
         GC.SuppressFinalize(this);
      }
   }
}