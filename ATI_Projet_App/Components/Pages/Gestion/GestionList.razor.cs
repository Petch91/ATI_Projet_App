using Microsoft.AspNetCore.Components;

namespace ATI_Projet_App.Components.Pages.Gestion
{
    public partial class GestionList : ComponentBase
    {
        [Inject] private HttpClient HttpClient { get; set; }
    }
}