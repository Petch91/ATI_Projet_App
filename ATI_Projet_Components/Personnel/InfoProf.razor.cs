using ATI_Projet_Models;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Microsoft.Extensions.Localization;
using ATI_Projet_Cultures.Locales;

namespace ATI_Projet_Components.Personnel
{
   public partial class InfoProf : ComponentBase
   {
      [Inject]
      private HttpClient HttpClient { get; set; }
      [Inject] private IStringLocalizer<PersonnelResource> localizer { get; set; }

      [Parameter]
      public EmployeProf EmployeProf { get; set; }
      [Parameter]
      public EventCallback ProfChanged { get; set; }

      private void EditProf(EmployeProf employeProf)
      {
         HttpClient.PatchAsJsonAsync<EmployeProf>("Employe/updateProf", employeProf);
         modal.HideAsync();
         ProfChanged.InvokeAsync();
         StateHasChanged();
      }

      private Modal modal = default!;

      private async Task ShowEditProf()
      {
         var parameters = new Dictionary<string, object>();
         parameters.Add("EmployeProf", EmployeProf);
         parameters.Add("EditProfEvent", EventCallback.Factory.Create<EmployeProf>(this, EditProf));
         await modal.ShowAsync<EditProf>(title: localizer["Edition des infos professionnelles"], parameters: parameters);
      }
   }
}