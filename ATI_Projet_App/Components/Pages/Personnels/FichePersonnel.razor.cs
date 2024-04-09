using ATI_Projets_Models;
using Microsoft.AspNetCore.Components;

namespace ATI_Projet_App.Components.Pages.Personnels
{
    public partial class FichePersonnel :ComponentBase
    {
        [Inject]
        private HttpClient client { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }


        public List<EmployeList> Liste { get; set; }

        public int Id { get; set; } 

        protected async override Task OnInitializedAsync()
        {
            Liste = await client.GetFromJsonAsync<List<EmployeList>>("Employe/") ?? new List<EmployeList>();
            Id = 54;
            //StateHasChanged();

        }
        void SelectChange(int id)
        {
            Id = id;
            StateHasChanged();
        }
    }
}