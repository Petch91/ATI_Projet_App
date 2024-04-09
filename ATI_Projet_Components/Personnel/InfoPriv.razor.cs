using ATI_Projet_Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;

namespace ATI_Projet_Components.Personnel
{
    public partial class InfoPriv : ComponentBase
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        [Parameter]
        public EmployePrivate EmployePrivate { get; set; }

        private Adresse Adresse { get; set; }

        protected async override Task OnParametersSetAsync()
        {
           if (EmployePrivate != null)
            {
                Adresse = new Adresse();
                Adresse = await HttpClient.GetFromJsonAsync<Adresse>("Employe/adresse/" +  EmployePrivate.AdresseId);
                Adresse.EmployeId = EmployePrivate.Id;
            }
        }
        public void EditPrivate()
        {
            HttpClient.PatchAsJsonAsync<EmployePrivate>("Employe/updatePrivate", EmployePrivate);
        }
        public void EditAdresse(Adresse adresse)
        {
            HttpClient.PatchAsJsonAsync<Adresse>("Employe/updateAdresse", adresse);
        }
    }
}