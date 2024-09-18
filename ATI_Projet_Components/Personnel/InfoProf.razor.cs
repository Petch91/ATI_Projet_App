using ATI_Projet_Models;
using BlazorBootstrapPerso;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Microsoft.Extensions.Localization;
using ATI_Projet_Cultures.Locales;
using ATI_Projet_Tools.Services.Interfaces;
using ATI_Projet_Cultures.Tools;

namespace ATI_Projet_Components.Personnel
{
   public partial class InfoProf : ComponentBase, IDisposable
   {
      [Inject] private IPersonnel personnel {  get; set; }
      [Inject] private IStringLocalizer<PersonnelResource> localizer { get; set; }
      [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

      [Parameter]
      public EmployeProf EmployeProf { get; set; }
      [Parameter]
      public EventCallback ProfChanged { get; set; }


      protected override void OnInitialized() => LanguageNotifier.SubscribeLanguageChange(this);
      public void Dispose() => LanguageNotifier.UnsubscribeLanguageChange(this);

      private async void EditProf(EmployeProf employeProf)
      { 
         await personnel.EditProf(employeProf);
         modal.HideAsync();
         await ProfChanged.InvokeAsync();
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