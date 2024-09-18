using ATI_Projet_Cultures.Tools;
using ATI_Projet_Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ATI_Projet_Components.Personnel
{
   public partial class EditProf : ComponentBase, IDisposable
   {
      [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

      [Parameter]
      public EmployeProf EmployeProf { get; set; }
      [Parameter]
      public EventCallback<EmployeProf> EditProfEvent { get; set; }

      protected override void OnInitialized() => LanguageNotifier.SubscribeLanguageChange(this);
      public void Dispose() => LanguageNotifier.UnsubscribeLanguageChange(this);

      public void Edit()
      {
         EditProfEvent.InvokeAsync(EmployeProf);

      }
   }
}