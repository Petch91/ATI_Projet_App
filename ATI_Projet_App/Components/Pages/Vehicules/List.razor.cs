using ATI_Projet_Models;
using ATI_Projet_Models.Models;
using Microsoft.AspNetCore.Components;

namespace ATI_Projet_App.Components.Pages.Vehicules
{
    public partial class List
    {
        [Inject]
        private HttpClient _client { get; set; }
        List<Vehicule> Vehicules { get; set; }
        protected async override Task OnInitializedAsync()
        {
          Vehicules =  await _client.GetFromJsonAsync<List<Vehicule>>("Vehicule/");
        }
    }
}