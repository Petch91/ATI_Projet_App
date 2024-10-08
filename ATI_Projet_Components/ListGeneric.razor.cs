using ATI_Projet_Models;
using ATI_Projet_Tools;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.ComponentModel;
using System.Reflection;
using ATI_Projet_Cultures.Locales;
using Microsoft.Extensions.Localization;
using ATI_Projet_Cultures.Tools;

namespace ATI_Projet_Components
{
   /// <summary>
   /// Liste qui accepte n'importe quel class pour faire un tableau 
   /// avec toutes les propriètés de cette class
   /// si le nom de la propriètés ne vous convient pas vous pouvez utiliser l'attribut DisplayName
   /// </summary>
   /// <typeparam name="TItem"></typeparam>
   public partial class ListGeneric<TItem> : ComponentBase, IDisposable where TItem : class
   {
      [Inject] private IStringLocalizer<PersonnelResource> localizer { get; set; }
      [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

      [Parameter]
      public IEnumerable<TItem> Items { get; set; }

      [Parameter]
      public List<string> ExcludedCols { get; set; }
      [Parameter]
      public bool ActivateOffCanvas { get; set; } = false;

      [Inject]
      private NavigationManager navigationManager { get; set; }

      private Dictionary<string, string> filters;

      private IQueryable<TItem> _liste = Enumerable.Empty<TItem>().AsQueryable();
      public IQueryable<TItem> Liste
      {
         get
         {
            var result = _liste;
            foreach (var keyValue in filters)
            {
               if (!string.IsNullOrEmpty(keyValue.Value))
               {
                  string name = keyValue.Key.Replace("filter", "");
                  result = result.Where(u => u.GetType().GetProperty(name).GetValue(u).ToString().Contains(keyValue.Value, StringComparison.CurrentCultureIgnoreCase));
               }
            }
            return result;
         }
      }

      PaginationState pagination = new PaginationState { ItemsPerPage = 15 };

      protected override void OnInitialized() => LanguageNotifier.SubscribeLanguageChange(this);
      public void Dispose() => LanguageNotifier.UnsubscribeLanguageChange(this);

      protected override void OnParametersSet()
      {
         if (ExcludedCols == null)
         {
            ExcludedCols = new List<string>();
         }
         filters = new Dictionary<string, string>();
         _liste = Items.AsQueryable();
         foreach (var p in _liste.ElementType.GetProperties())
         {
            if (p.PropertyType == typeof(string))
            {
               filters.Add($"{p.Name}filter", "");
            }
         }
      }

      public void Show(int id)
      {

         Type type = typeof(TItem);
         string name = type.GetCustomAttribute<DisplayNameAttribute>() == null ? type.Name : type.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
         if (ActivateOffCanvas)
         {
            navigationManager.NavigateTo($"{name}/" + id);
         }
         else
         {
            navigationManager.NavigateTo($"{name}/show/" + id);
         }

      }

      private string GetLocalString(object name)
      {
         return name != null ? localizer[name.ToString() ?? ""] : "";
      }
   }
}