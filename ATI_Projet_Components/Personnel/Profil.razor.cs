using ATI_Projet_Models;
using ATI_Projet_Models.Models.Forms;
using ATI_Projet_Models.Types;
using ATI_Projet_Models.VCA;
using BlazorBootstrap;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace ATI_Projet_Components.Personnel
{
    enum GestionStock
    {
        NON = 0,
        Minimum = 1,
        Moyen = 5,
        Total = 9

    }
    public partial class Profil
    {
        [Inject]
        private HttpClient httpClient { get; set; } =default!;
        [Parameter]
        public EmployeProfil EmployeProfil { get; set; }
        private List<string> ChoixTitre = new List<string> { "", "M.", "Mme", "X." };

        private IEnumerable<Fonction> fonctions;
        private IEnumerable<FonctionVCA> fonctionsVCA;
        private IEnumerable<StatusVCA> statusVCA;
        private IEnumerable<TypeContrat> TypesContrat;
        private IEnumerable<TypeMO> TypesMO;
        private IEnumerable<DeptSimplifiedList> departements;


        private List<Email> emails;
        private List<Telephone> telephones;
        private List<Email> tempEmails;
        private List<Telephone> tempTelephones;

        private Offcanvas offcanvas = default!;

        protected async override Task OnInitializedAsync()
        {
            fonctions = new List<Fonction>();
            fonctions = await httpClient.GetFromJsonAsync<IEnumerable<Fonction>>("Fonction");
            fonctionsVCA = new List<FonctionVCA>();
            fonctionsVCA = await httpClient.GetFromJsonAsync<IEnumerable<FonctionVCA>>("VCA/fonction");
            statusVCA = new List<StatusVCA>();
            statusVCA = await httpClient.GetFromJsonAsync<IEnumerable<StatusVCA>>("VCA/status");
            TypesContrat = new List<TypeContrat>();
            TypesContrat = await httpClient.GetFromJsonAsync<IEnumerable<TypeContrat>>("Type/contrat");
            TypesMO = new List<TypeMO>();
            TypesMO = await httpClient.GetFromJsonAsync<IEnumerable<TypeMO>>("Type/MO");
            departements = new List<DeptSimplifiedList>();
            departements = await httpClient.GetFromJsonAsync<IEnumerable<DeptSimplifiedList>>("departement/simplifiedList");
        }

        protected async override Task OnParametersSetAsync()
        {
            emails = new List<Email>();
            telephones = new List<Telephone>();
            if (EmployeProfil != null)
            {               
                emails = await httpClient.GetFromJsonAsync<List<Email>>("Employe/emailsByEmploye/" + EmployeProfil.PersonneId);

                telephones = await httpClient.GetFromJsonAsync<List<Telephone>>("Employe/telephonesByEmploye/" + EmployeProfil.PersonneId);
            }
        }

        public void Edit()
        {
            httpClient.PatchAsJsonAsync<EmployeProfil>("Employe/updateProfil", EmployeProfil);
        }
        public async void OpenCanvas()
        {
            tempEmails = new List<Email>();
            foreach (Email email in emails)
            {
                tempEmails.Add(new Email(email));
            }

            tempTelephones = new List<Telephone>();
            foreach (Telephone telephone in telephones)
            {
                tempTelephones.Add(new Telephone(telephone));
            }
            await offcanvas.ShowAsync();
        }
        public async void OnHideOffcanvasClick()
        {
            await offcanvas.HideAsync();
        }
        public void EditMail(Email email)
        {
            int index = emails.IndexOf(emails.First(e => e.Id == email.Id));
            emails[index] = email;
            httpClient.PutAsJsonAsync<Email>("Employe/updateEmail", email);
            StateHasChanged();

        }
        public void EditTelephone(Telephone telephone)
        {
            int index = telephones.IndexOf(telephones.First(e => e.Id == telephone.Id));
            telephones[index] = telephone;
            httpClient.PutAsJsonAsync<Telephone>("Employe/updateTelephone", telephone);
            StateHasChanged();
        }

        public void AddNumber(int id)
        {
            tempTelephones.Add(new Telephone { PersonneId = id });
            StateHasChanged();
        }
        public void AddMail(int id)
        {
            tempEmails.Add(new Email { PersonneId = id });
            StateHasChanged();
        }
    }
}