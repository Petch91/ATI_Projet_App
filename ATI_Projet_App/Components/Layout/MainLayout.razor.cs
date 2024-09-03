
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using ATI_Projet_App.Models;
using ATI_Projet_Tools;
using ATI_Projet_Models;
using ATI_Projet_Tools.Services.Interfaces;

namespace ATI_Projet_App.Components.Layout
{
   public partial class MainLayout
   {
      [Inject] private IPersonnel personnel { get; set; }

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
            if (result.Value != null) connectedUser = await personnel.GotProfil(result.Value.IdExterne);
            StateHasChanged();
         }
      }

        public void GoLogin()
        {
            navigationManager.NavigateTo("http://192.168.123.238:7100/", true);
        }
    }
}