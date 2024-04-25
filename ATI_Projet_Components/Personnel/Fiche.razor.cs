using ATI_Projet_Models;
using ATI_Projet_Models.Models.Forms;
using ATI_Projet_Models.Types;
using ATI_Projet_Models.VCA;
using ATI_Projets_Models;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http.Json;

namespace ATI_Projet_Components.Personnel
{
    public partial class Fiche : ComponentBase, IDisposable
    {
        [Inject] private HttpClient httpClient { get; set; } = default!;
        [Parameter] public EventCallback<int> EditPhoto { get; set; }
        [Parameter] public EventCallback<int> EditSignature { get; set;} 
        
        private int Id { get; set; }

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

        private Offcanvas offcanvas = default!;

        CancellationTokenSource cts = new CancellationTokenSource();

        // Créer un jeton d'annulation à partir du CancellationTokenSource
        CancellationToken token = default!;

        // Lancer une tâche asynchrone qui utilise le jeton d'annulation
        Task task = default!;
        protected async override Task OnInitializedAsync()
        {
            Liste = await httpClient.GetFromJsonAsync<List<EmployeList>>("Employe/") ?? new List<EmployeList>();
            if (Liste != null) Id = Liste.First().Id;
            fonctions = new List<Fonction>();
            fonctions = await httpClient.GetFromJsonAsync<IEnumerable<Fonction>>("Fonction");
            departements = new List<DeptSimplifiedList>();
            departements = await httpClient.GetFromJsonAsync<IEnumerable<DeptSimplifiedList>>("departement/simplifiedList");
            token = cts.Token;
            task = Task.Run(async () =>
            {
                await Task.Delay(300, token);
            }, token);
        }

        protected async override Task OnParametersSetAsync()
        {
            await ChangeEmploye();
        }
        private async void SelectChange(int id)
        {
            Id = id;
            await task;
            if(!task.IsCanceled)
            {
                
                await ChangeEmploye();
                StateHasChanged();
            }

        }

        private void choisir(int choice)
        {
            choix = choice;
            StateHasChanged();
        }

        public async void ShowCanvas()
        {
            await offcanvas.ShowAsync();
        }
        public async void OnHideOffcanvasClick()
        {
            await offcanvas.HideAsync();
        }
        public async void TriggerEmail()
        {
            emails = await httpClient.GetFromJsonAsync<List<Email>>("Employe/emailsByEmploye/" + EmployeProfil.PersonneId);
            StateHasChanged();
        }

        public async void TriggerTelephone()
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
            PhotoPath = string.IsNullOrEmpty(EmployePrivate.Photo) ? "/images/Photo.jpg" : (Path.GetFullPath(EmployePrivate.Photo)).Replace("C:\\Users\\ati_etu3\\source\\repos\\ATI_Projet_App\\ATI_Projet_App\\wwwroot", "").Replace("\\", "/");
            SignaturePath = string.IsNullOrEmpty(EmployePrivate.Signature) ? "/images/signature.webp" : (Path.GetFullPath(EmployePrivate.Signature)).Replace("C:\\Users\\ati_etu3\\source\\repos\\ATI_Projet_App\\ATI_Projet_App\\wwwroot", "").Replace("\\", "/");

            EmployeProf = await httpClient.GetFromJsonAsync<EmployeProf>("Employe/prof/" + Id) ?? new EmployeProf();

            if (EmployeProfil != null)
            {
                emails = await httpClient.GetFromJsonAsync<List<Email>>("Employe/emailsByEmploye/" + EmployeProfil.PersonneId);

                telephones = await httpClient.GetFromJsonAsync<List<Telephone>>("Employe/telephonesByEmploye/" + EmployeProfil.PersonneId);
            }
            
        }

        private async void ChangePhoto()
        {
            await EditPhoto.InvokeAsync(Id);

        }

        private async void ChangeSignature()
        {
            await EditSignature.InvokeAsync(Id);
        }

        private Modal modal { get; set; }
        private async void OpenModalCreate()
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("CreateEvent", EventCallback.Factory.Create<PersonneForm>(this, AddEmploye));
            await modal.ShowAsync<CreatePersonne>(title: @"Ajouter un employé", parameters: parameters);
        }

        private async void AddEmploye(PersonneForm personne)
        {
            await httpClient.PostAsJsonAsync<PersonneForm>("Employe/", personne);
            await modal.HideAsync();
            Liste = await httpClient.GetFromJsonAsync<List<EmployeList>>("Employe/") ?? new List<EmployeList>();
            if (Liste != null) Id = Liste.Last().Id;
            await ChangeEmploye();
            StateHasChanged();
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}