using ATI_Projet_App.Models;
using ATI_Projet_App.Tools;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ATI_Projet_App.Components.Layout
{
    public partial class NavMenu
    {
        [Inject]
        private SessionManager session {  get; set; }

        [Inject]
        private ProtectedLocalStorage storage { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }

        private User? connectedUser;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //return base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var result = await storage.GetAsync<User>("ConnectedUser");
                connectedUser = result.Value;
                StateHasChanged();
            }
        }
        private async Task Logout()
        {
            await session.Logout();
            navigationManager.NavigateTo("http://192.168.122.77:7100/logout", true);
            //navigationManager.NavigateTo("/login",true);
        }
    }
}