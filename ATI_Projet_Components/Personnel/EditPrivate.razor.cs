using ATI_Projet_Cultures.Tools;
using ATI_Projet_Models;
using ATI_Projet_Models.Events;
using Microsoft.AspNetCore.Components;

namespace ATI_Projet_Components.Personnel
{
   public partial class EditPrivate : IDisposable
   {
      [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

      [Parameter]
      public EmployePrivate EmployePrivate { get; set; }
      [Parameter]
      public Adresse Adresse { get; set; }

      [Parameter]
      public Dictionary<string, string> Pays { get; set; }

      [Parameter]
      public Dictionary<string, string> Langues { get; set; }

      [Parameter]
      public Dictionary<string, string> Nationalites { get; set; }

      [Parameter]
      public EventCallback<EditPrivateArgs> EditEmployePrivateEvent { get; set; }


      protected override void OnInitialized() => LanguageNotifier.SubscribeLanguageChange(this);
      public void Dispose() => LanguageNotifier.UnsubscribeLanguageChange(this);

      public async Task EditEmployePrivate()
      {
         await EditEmployePrivateEvent.InvokeAsync(new EditPrivateArgs { Employe = EmployePrivate, Adresse = Adresse });
      }

   }
}