using ATI_Projet_Models;
using ATI_Projet_Models.Types;
using ATI_Projet_Models.VCA;
using ATI_Projets_Models;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace ATI_Projet_Components.Personnel
{
    public partial class Fiche
    {
        [Inject] private HttpClient httpClient { get; set; } = default!;
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

        private Offcanvas offcanvas = default!;

        protected async override Task OnInitializedAsync()
        {
            Liste = await httpClient.GetFromJsonAsync<List<EmployeList>>("Employe/") ?? new List<EmployeList>();
            if (Liste != null) Id = Liste.First().Id;
            fonctions = new List<Fonction>();
            fonctions = await httpClient.GetFromJsonAsync<IEnumerable<Fonction>>("Fonction");
            departements = new List<DeptSimplifiedList>();
            departements = await httpClient.GetFromJsonAsync<IEnumerable<DeptSimplifiedList>>("departement/simplifiedList");
        }

        protected async override Task OnParametersSetAsync()
        {
            await ChangeEmploye();
        }
        private async void SelectChange(int id)
        {
            Id = id;
            await ChangeEmploye();
            StateHasChanged();
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
            EmployeProfil = new EmployeProfil();
            EmployeProfil = await httpClient.GetFromJsonAsync<EmployeProfil>("Employe/profil/" + Id) ?? new EmployeProfil();
            EmployePrivate = new EmployePrivate();
            EmployePrivate = await httpClient.GetFromJsonAsync<EmployePrivate>("Employe/private/" + Id) ?? new EmployePrivate();
            EmployePrivate.Photo = string.IsNullOrEmpty(EmployePrivate.Photo) ? "/images/Photo.jpg" : EmployePrivate.Photo;
            EmployePrivate.Signature = string.IsNullOrEmpty(EmployePrivate.Signature) ? "/images/signature.webp" : EmployePrivate.Signature;
            EmployeProf = new EmployeProf();
            EmployeProf = await httpClient.GetFromJsonAsync<EmployeProf>("Employe/prof/" + Id) ?? new EmployeProf();
            emails = new List<Email>();
            telephones = new List<Telephone>();
            if (EmployeProfil != null)
            {
                emails = await httpClient.GetFromJsonAsync<List<Email>>("Employe/emailsByEmploye/" + EmployeProfil.PersonneId);

                telephones = await httpClient.GetFromJsonAsync<List<Telephone>>("Employe/telephonesByEmploye/" + EmployeProfil.PersonneId);
            }
        }

    }
}