using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Reflection;
using System.Resources;

namespace ATI_Projet_Cultures.Tools
{
   public class CustomStringLocalizer<TComponent> : IStringLocalizer<TComponent>
   {
      public LocalizedString this[string name] => FindLocalziedString(name);
      public LocalizedString this[string name, params object[] arguments] => FindLocalziedString(name, arguments);
      private readonly IOptions<LocalizationOptions> _localizationOptions;
      private readonly LanguageChangeNotifier _languageChangeNotifier;

      public CustomStringLocalizer(IOptions<LocalizationOptions> localizationOptions, LanguageChangeNotifier languageChangeNotifier)
      {
         _localizationOptions = localizationOptions;
         _languageChangeNotifier = languageChangeNotifier;
      }

      public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
      {
         var resourceManager = CreateResourceManager();
         var result = new List<LocalizedString>();

         try
         {
            var resourceSet = resourceManager.GetResourceSet(_languageChangeNotifier.CurrentCulture, true, true);
            result = resourceSet.Cast<DictionaryEntry>()
                .Select(item => new LocalizedString((string)item.Key, (string)item.Value, false, GetResourceLocaltion()))
                .ToList();
         }
         catch
         {
            result.Add(new("", "", true, GetResourceLocaltion()));
         }

         return result;
      }

      private LocalizedString FindLocalziedString(string key, object[]? arguments = default)
      {
         var resourceManager = CreateResourceManager();
         LocalizedString result;

         try
         {
            string value = resourceManager.GetString(key, _languageChangeNotifier.CurrentCulture);

            if (arguments is not null)
            {
               value = string.Format(value, arguments);
            }

            result = new(key, value, false, GetResourceLocaltion());
         }
         catch
         {
            result = new(key, key, true, GetResourceLocaltion());
         }

         return result;
      }

      private ResourceManager CreateResourceManager()
      {
         string resourceLocaltion = GetResourceLocaltion();
         var resourceManager = new ResourceManager(resourceLocaltion, Assembly.GetExecutingAssembly());

         return resourceManager;
      }

      private string GetResourceLocaltion()
      {
         var componentType = typeof(TComponent);
         var nameParts = componentType.FullName.Split('.').ToList();
         if(!string.IsNullOrEmpty(_localizationOptions.Value.ResourcesPath)) nameParts.Insert(1, _localizationOptions.Value.ResourcesPath);
         string resourceLocaltion = string.Join(".", nameParts);

         return resourceLocaltion;
      }
   }
}
