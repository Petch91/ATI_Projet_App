using ATI_Projet_Models;
using ATI_Projet_Tools;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using System.Net.Http.Json;

namespace ATI_Projet_Components
{
    public partial class ListePerso : ComponentBase
    {
        [Inject]
        private  ApiRequester api {  get; set; }
        [Inject]
        private HttpClient client { get; set; }

        private string nameFilter;
        private  IQueryable<Personnel> _liste= Enumerable.Empty<Personnel>().AsQueryable();
        public List<Personnel> Temp { get; set; } = new List<Personnel>();
        public IQueryable<Personnel> Liste {
                                                get 
                                                {
                                                    if(!string.IsNullOrEmpty(nameFilter))
                                                    {
                                                        return _liste.Where(u => u.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase));
                                                    }
                                                    return _liste;
                                                }
                                           }

        PaginationState pagination = new PaginationState { ItemsPerPage = 25 };

        protected async override Task OnInitializedAsync()
        {
            Temp = await client.GetFromJsonAsync<List<Personnel>>("personnel/");
           _liste = Temp.AsQueryable();
        }


    }
}