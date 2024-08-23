
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using ATI_Projet_App.Models;
using ATI_Projet_Tools;
using ATI_Projet_Models;

namespace ATI_Projet_App.Components.Layout
{
    public partial class MainLayout
    {
        [Inject]
        private HttpClient httpClient { get; set; }

        [Inject]
        private ProtectedLocalStorage storage { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }

        private EmployeProfil? connectedUser;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //return base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var result = await storage.GetAsync<User>("ConnectedUser");
                if(result.Value != null) connectedUser = await httpClient.GetFromJsonAsync<EmployeProfil>("Employe/Profil/" + result.Value.IdExterne);
                StateHasChanged();
            }
        }

        public void GoLogin()
        {
            navigationManager.NavigateTo("http://192.168.123.69/", true);
        }
    }
}