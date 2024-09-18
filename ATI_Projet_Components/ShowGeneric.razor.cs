using ATI_Projet_Cultures.Tools;
using Blazorise;
using Microsoft.AspNetCore.Components;

namespace ATI_Projet_Components
{
   public partial class ShowGeneric<TItem> : ComponentBase, IDisposable where TItem : class
   {

      [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

      [Parameter]
      public TItem Item { get; set; }

      [Parameter]
      public List<string> ExcludedProp { get; set; }

      protected override void OnInitialized() => LanguageNotifier.SubscribeLanguageChange(this);
      public void Dispose() => LanguageNotifier.UnsubscribeLanguageChange(this);

      protected override void OnParametersSet()
      {
         if (ExcludedProp == null)
         {
            ExcludedProp = new List<string>();
         }
      }
   }
}