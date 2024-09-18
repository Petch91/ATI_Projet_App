using ATI_Projet_Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using ATI_Projet_Cultures.Tools;

namespace ATI_Projet_Components.Telephones
{
   public partial class TelephoneForm : IDisposable
   {
      [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }
      [Parameter]
      public Telephone Telephone { get; set; }
      [Parameter]
      public EventCallback<Telephone> OnValidation { get; set; }

      private List<string> descriptions = new List<string> { "Fixe Privé", "Fixe Professionnel", "Gsm Privé", "Gsm Professionnel" };


      protected override void OnInitialized() => LanguageNotifier.SubscribeLanguageChange(this);
      public void Dispose() => LanguageNotifier.UnsubscribeLanguageChange(this);

      public void IsValided()
      {

         OnValidation.InvokeAsync(Telephone);

      }

   }
}