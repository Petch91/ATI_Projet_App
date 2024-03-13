using ATI_Projet_Models;
using Microsoft.AspNetCore.Components;

namespace ATI_Projet_App.Components.Pages.Personnels
{
    public partial class Show : ComponentBase
    {
        [Inject]
        private HttpClient http {  get; set; }

        [Parameter]
        public string Id { get; set; }

        private Personnel Personne { get; set; }

        protected async override void OnParametersSet()
        {
            Personne = await http.GetFromJsonAsync<Personnel>("personnel/"+Id) ?? new Personnel();
            StateHasChanged();
        }
    }
}