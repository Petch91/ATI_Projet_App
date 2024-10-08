using ATI_Projet_Models;
using ATI_Projet_Tools;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.ComponentModel;
using System.Reflection;
using ATI_Projet_Cultures.Locales;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System.IO.Pipelines;
using ATI_Projet_Cultures.Tools;
using Blazorise.Extensions;

namespace ATI_Projet_Components
{
   /// <summary>
   /// Liste qui accepte n'importe quel class pour faire un tableau 
   /// avec toutes les propriètés de cette class
   /// si le nom de la propriètés ne vous convient pas vous pouvez utiliser l'attribut DisplayName
   /// </summary>
   /// <typeparam name="TItem"></typeparam>
   public partial class ListGenericEdit<TItem, TId> : ComponentBase, IDisposable where TItem : class,new()
   {
      [Inject] private IStringLocalizer<PersonnelResource> localizer {  get; set; }
      [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

      [Parameter]
      public IEnumerable<TItem> Items { get; set; }
      [Parameter]
      public List<string> ExcludedCols { get; set; }
      [Parameter]
      public EventCallback<object> EditEvent { get; set; }
      [Parameter]
      public EventCallback<object> DeleteEvent { get; set; }
      [Parameter]
      public int ItemsPerPageForPagination { get; set; }

      [Parameter]
      public string IdName { get; set; }

      private bool loadingEdit = false;
      private bool loadingDelete = false;

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
                  result = result.Where(u => localizer[u.GetType().GetProperty(name).GetValue(u).ToString()].ToString().Contains(localizer[keyValue.Value], StringComparison.CurrentCultureIgnoreCase));
               }
            }
            return result.IsNullOrEmpty() ? new List<TItem>{new ()}.AsQueryable() : result ;
         }
      }

      private PaginationState pagination;


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
         if (ItemsPerPageForPagination == null || ItemsPerPageForPagination == 0)
         {
            ItemsPerPageForPagination = 15;
         }
         pagination = new PaginationState { ItemsPerPage = ItemsPerPageForPagination };

         if (string.IsNullOrEmpty(IdName))
         {
            IdName = "Id";
         }
      }

      public async Task Edit(TId id)
      {
         loadingEdit = true;
         await EditEvent.InvokeAsync(id);
         loadingEdit = false;
      }

      public async Task Delete(TId id)
      {
         loadingDelete = true;
         await DeleteEvent.InvokeAsync(id);
         loadingDelete = false;
      }

      private string GetLocalString(object name)
      {
         return name != null ?  localizer[name.ToString()??""] : "";
      }

   }
}