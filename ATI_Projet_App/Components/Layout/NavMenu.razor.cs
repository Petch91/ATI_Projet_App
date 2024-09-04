using ATI_Projet_App.Models;
using ATI_Projet_App.Tools;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Globalization;

namespace ATI_Projet_App.Components.Layout
{
   public partial class NavMenu
   {
      [Inject]
      private SessionManager session { get; set; }

      [Inject]
      private ProtectedLocalStorage storage { get; set; }
      [Inject]
      private NavigationManager navigationManager { get; set; }

      private User? connectedUser;

      private string drapeauFR;
      private string drapeauUS;

      private CultureInfo culture
      {
         get
         {
            return CultureInfo.CurrentCulture;
         }

         set
         {
            if (value != null && CultureInfo.CurrentCulture != value)
            {
               var uri = new Uri(navigationManager.Uri).GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
               var cultureEscaped = Uri.EscapeDataString(value.Name);
               var uriEscaped = Uri.EscapeDataString(uri);

               navigationManager.NavigateTo($"Culture/Set?culture={cultureEscaped}&redirectUri={uriEscaped}", true);
            }
         }
      }

      protected override async void OnInitialized()
      {
         culture = CultureInfo.CurrentCulture;
         StateHasChanged();
      }

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
         navigationManager.NavigateTo("http://ati-portal.be/logout", true);
         //navigationManager.NavigateTo("/login",true);
      }
   }
}