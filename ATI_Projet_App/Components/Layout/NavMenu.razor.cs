using ATI_Projet_App.Models;
using ATI_Projet_App.Tools;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Globalization;
using ATI_Projet_Cultures.Tools;
using Microsoft.Extensions.Options;

namespace ATI_Projet_App.Components.Layout
{
   public partial class NavMenu : IDisposable
   {
      [Inject] private IOptions<RequestLocalizationOptions> options { get; set; }
      [Inject]
      private SessionManager session { get; set; }

      [Inject]
      private ProtectedLocalStorage storage { get; set; }
      [Inject]
      private NavigationManager navigationManager { get; set; }

      [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

      private User? connectedUser;

      private string culture;


      protected override void OnInitialized() 
      { 
         LanguageNotifier.SubscribeLanguageChange(this); 
         culture = options.Value.DefaultRequestCulture.Culture.ToString();
      }
      public void Dispose() => LanguageNotifier.UnsubscribeLanguageChange(this);

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

      private async Task OnChangeLanguageAsync(ChangeEventArgs e)
      {
         string selectedCulture = e.Value as string;

         if (!string.IsNullOrEmpty(selectedCulture))
         {
            await storage.SetAsync("Language", selectedCulture);
            options.Value.SetDefaultCulture(selectedCulture);
            LanguageNotifier.CurrentCulture = CultureInfo.GetCultureInfo(selectedCulture);
            //StateHasChanged();
         }
      }
   }
}