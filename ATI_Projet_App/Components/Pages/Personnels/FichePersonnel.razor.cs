using ATI_Projet_App.Tools;
using ATI_Projet_Components.Personnel;
using ATI_Projet_Cultures.Tools;
using ATI_Projet_Models;
using ATI_Projet_Tools.Services.Interfaces;
using ATI_Projets_Models;
using BlazorBootstrapPerso;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ATI_Projet_App.Components.Pages.Personnels
{
   public partial class FichePersonnel : ComponentBase, IDisposable
   {
      [Inject] private IPersonnel personnel {  get; set; }
      [Inject] private NavigationManager navigationManager { get; set; } = default!;
      [Inject] private SessionManager session {  get; set; } = default!;
      [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

      [Parameter] public int ID { get; set; }

      //private Modal modalPhoto;

      //private Modal modalSignature;

      protected override void OnInitialized() => LanguageNotifier.SubscribeLanguageChange(this);
      public void Dispose() => LanguageNotifier.UnsubscribeLanguageChange(this);

      protected override async Task OnParametersSetAsync()
      {
         if (ID == 0)
         {
            int? id = await session.GetSessionStorage<int>("CurrentId");
            if (id != null && id > 0) ID = (int)id;
            else ID = -1;
         }
         StateHasChanged();
      }

      private async Task OpenModalPhoto(int id)
      {
         ID = id;
         var parameters = new Dictionary<string, object>();
         //parameters.Add("UploadPath", @$"C:\Users\ati_etu3\source\repos\ATI_Projet_App\ATI_Projet_App\wwwroot\images\photos\employe{id}");
         parameters.Add("UploadPath", @$"/app/wwwroot/images/photos/employe{id}");
         parameters.Add("PhotoUploaded", EventCallback.Factory.Create<string>(this, EditPhoto));
         //await modalPhoto.ShowAsync<UploadPhoto>(title: localizer["Changer La Photo"], parameters: parameters);
      }
      private async Task EditPhoto(string path)
      {
         await personnel.EditPhoto(ID, path);
         //await modalPhoto.HideAsync();
         StateHasChanged();
         //navigationManager.Refresh(); 
      }
      private async Task OpenModalSignature(int id)
      {
         ID = id;
         var parameters = new Dictionary<string, object>();
         parameters.Add("UploadPath", @$"/app/wwwroot/images/signatures/employe{id}");
         parameters.Add("PhotoUploaded", EventCallback.Factory.Create<string>(this, EditSignature));
         //await modalSignature.ShowAsync<UploadPhoto>(title: localizer["Changer La Signature"], parameters: parameters);
      }
      private async Task EditSignature(string path)
      {
         await personnel.EditSignature(ID, path);
         //await modalSignature.HideAsync();
         navigationManager.Refresh();
      }

      private void EmployeEvent(int id)
      {
         session.SetSessionStorage<int>("CurrentId", id);
         ID = id;

      }
   }
}