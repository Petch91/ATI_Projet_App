using ATI_Projet_App.Tools;
using ATI_Projet_Components.Personnel;
using ATI_Projet_Models;
using ATI_Projets_Models;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ATI_Projet_App.Components.Pages.Personnels
{
   public partial class FichePersonnel : ComponentBase
   {
      [Inject] private HttpClient httpClient { get; set; } = default!;
      [Inject] private NavigationManager navigationManager { get; set; } = default!;
      [Inject] private SessionManager session {  get; set; } = default!;

      private Modal modalPhoto;

      private Modal modalSignature;

      private int ID;

      protected override async void OnInitialized()
      {
         int? id = await session.GetSessionStorage<int>("CurrentId");
         if (id != null && id > 0) ID = (int)id;
         else ID = -1;
         //StateHasChanged();
      }
      protected override void OnAfterRender(bool firstRender)
      {
         if (firstRender)
         {
            StateHasChanged();
         }
      }

      private async Task OpenModalPhoto(int id)
      {
         ID = id;
         var parameters = new Dictionary<string, object>();
         //parameters.Add("UploadPath", @$"C:\Users\ati_etu3\source\repos\ATI_Projet_App\ATI_Projet_App\wwwroot\images\photos\employe{id}");
         parameters.Add("UploadPath", @$"/app/wwwroot/images/photos/employe{id}");
         parameters.Add("PhotoUploaded", EventCallback.Factory.Create<string>(this, EditPhoto));
         await modalPhoto.ShowAsync<UploadPhoto>(title: "Changer La Photo", parameters: parameters);
      }
      private async Task EditPhoto(string path)
      {
         await httpClient.PatchAsJsonAsync("Employe/changePhoto", new { Id = ID, Path = path });
         await modalPhoto.HideAsync();
         StateHasChanged();
         //navigationManager.Refresh();
      }
      private async Task OpenModalSignature(int id)
      {
         ID = id;
         var parameters = new Dictionary<string, object>();
         parameters.Add("UploadPath", @$"/app/wwwroot/images/signatures/employe{id}");
         parameters.Add("PhotoUploaded", EventCallback.Factory.Create<string>(this, EditSignature));
         await modalSignature.ShowAsync<UploadPhoto>(title: "Changer La Signature", parameters: parameters);
      }
      private async Task EditSignature(string path)
      {
         await httpClient.PatchAsJsonAsync("Employe/changeSignature", new { Id = ID, Path = path });
         await modalSignature.HideAsync();
         navigationManager.Refresh();
      }

      private void EmployeEvent(int id)
      {
         session.SetSessionStorage<int>("CurrentId", id);
         ID = id;

      }
   }
}