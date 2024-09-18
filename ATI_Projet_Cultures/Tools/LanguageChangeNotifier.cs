using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;

namespace ATI_Projet_Cultures.Tools
{
   public class LanguageChangeNotifier
   {
      private readonly List<ComponentBase> _subscribedComponents = new();
      private CultureInfo _currentCulture;

      public CultureInfo CurrentCulture
      {
         get => _currentCulture;

         set
         {
            if (string.IsNullOrWhiteSpace(value.Name))
            {
               _currentCulture = CultureInfo.CurrentCulture;
               NotifyLanguageChange();
            }
            else
            {
               var allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

               if (allCultures.Contains(value))
               {
                  _currentCulture = value;
                  NotifyLanguageChange();
               }
            }
         }
      }

      public void SubscribeLanguageChange(ComponentBase component) => _subscribedComponents.Add(component);

      public void UnsubscribeLanguageChange(ComponentBase component) => _subscribedComponents.Remove(component);

      public LanguageChangeNotifier(IOptions<RequestLocalizationOptions> options)
      {
         _currentCulture = options.Value.DefaultRequestCulture.Culture;
      }

      private void NotifyLanguageChange()
      {
         foreach (var component in _subscribedComponents)
         {
            if (component is not null)
            {
               var componentType = component.GetType();
               try
               {
                  var stateHasChangedMethod = componentType?.GetMethod("StateHasChanged", BindingFlags.Instance | BindingFlags.NonPublic);
                  _ = (stateHasChangedMethod?.Invoke(component, null));
               }
               catch (Exception ex)
               {
                  Console.WriteLine(componentType);
               }
            }
         }
      }
   }
}
