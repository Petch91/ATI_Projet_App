using ATI_Projet_Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace ATI_Projet_App.Components.Pages.Personnels
{
    public partial class Search : ComponentBase
    {

        [Inject]
        private HttpClient client { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }

        private Personnel personne {  get; set; } 

        public List<Personnel> Liste { get; set; }
     
        public int Id { get; set; } = 0;

        protected async override Task OnInitializedAsync()
        {
            Liste = await client.GetFromJsonAsync<List<Personnel>>("Personnel/") ??  new List<Personnel>();

        }
         void SelectChange(int id)
        {
            personne = Liste.Where(x => x.Id == id).First();
            StateHasChanged();
        }
    }
}