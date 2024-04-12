using ATI_Projet_Models;
using ATI_Projet_Models.Models.Forms;
using ATI_Projet_Models.Types;
using ATI_Projet_Models.VCA;
using BlazorBootstrap;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
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


        public void Edit(EmployeProfil employeProfil)
        {
            EmployeProfil = employeProfil;
            httpClient.PatchAsJsonAsync<EmployeProfil>("Employe/updateProfil", employeProfil);
            modal.HideAsync();
            ProfilChanged.InvokeAsync(EmployeProfil);
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
            await modal.ShowAsync<EditProfil>(title: "Edition du Profil", parameters: parameters);
        }

    }
}