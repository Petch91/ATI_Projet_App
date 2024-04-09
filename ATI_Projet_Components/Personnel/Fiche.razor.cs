using ATI_Projet_Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace ATI_Projet_Components.Personnel
{
    public partial class Fiche
    {
        [Inject]
        private HttpClient httpClient {  get; set; } = default!;
        [Parameter]
        public string Id { get; set; }

        private EmployeProfil EmployeProfil { get; set; }
        private EmployePrivate EmployePrivate { get; set; }
        private EmployeProf EmployeProf { get; set; }

        private int choix = 0;

        protected async override Task OnParametersSetAsync()
        {
            if (int.Parse(Id) < 54) 
            {
                Id = "54";
            }
            EmployeProfil = new EmployeProfil();
            EmployeProfil = await httpClient.GetFromJsonAsync<EmployeProfil>("Employe/profil/" + Id) ?? new EmployeProfil();
            EmployePrivate = new EmployePrivate();
            EmployePrivate = await httpClient.GetFromJsonAsync<EmployePrivate>("Employe/private/" + Id) ?? new EmployePrivate();
            EmployeProf = new EmployeProf();
            EmployeProf = await httpClient.GetFromJsonAsync<EmployeProf>("Employe/prof/" + Id) ?? new EmployeProf();

        }

        private void choisir(int choice)
        {
            choix = choice;
            StateHasChanged();
        }
    }
}