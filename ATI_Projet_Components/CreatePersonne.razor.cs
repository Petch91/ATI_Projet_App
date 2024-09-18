using ATI_Projet_Cultures.Tools;
using ATI_Projet_Models.Models.Forms;
using Microsoft.AspNetCore.Components;

namespace ATI_Projet_Components
{
   public partial class CreatePersonne : ComponentBase, IDisposable
   {
      [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

      [Parameter] public EventCallback<PersonneForm> CreateEvent { get; set; }

      private PersonneForm form = new PersonneForm();

      [Parameter]
      public Dictionary<string, string> Langues { get; set; }

      [Parameter]
      public Dictionary<string, string> Nationalites { get; set; }

      private List<string> ChoixTitre = new List<string> { "M.", "Mme", "X." };

      protected override void OnInitialized() => LanguageNotifier.SubscribeLanguageChange(this);
      public void Dispose() => LanguageNotifier.UnsubscribeLanguageChange(this);

      private async Task Submit()
      {
         await CreateEvent.InvokeAsync(form);
         form = new PersonneForm();
         StateHasChanged();
      }
   }
}