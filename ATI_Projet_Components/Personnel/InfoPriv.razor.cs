using ATI_Projet_Models;
using ATI_Projet_Models.Types;
using ATI_Projet_Models.VCA;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ATI_Projet_Components.Personnel
{
    public partial class InfoPriv : ComponentBase
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        [Parameter]
        public EmployePrivate EmployePrivate { get; set; }

        private Adresse Adresse { get; set; }

        private Dictionary<string, string> Pays { get; set; }

        protected async override void OnInitialized()
        {
            Pays = await HttpClient.GetFromJsonAsync<Dictionary<string, string>>("https://www.iso-country-code.com/api/?lang=fr&output=json");
        }

        protected async override Task OnParametersSetAsync()
        {
            if (EmployePrivate != null)
            {
                Adresse = new Adresse();
                Adresse = await HttpClient.GetFromJsonAsync<Adresse>("Employe/adresse/" + EmployePrivate.AdresseId);
                Adresse.EmployeId = EmployePrivate.Id;
            }
        }
        public void EditPrivate(EmployePrivate employePrivate)
        {
            EmployePrivate = employePrivate;
            HttpClient.PatchAsJsonAsync<EmployePrivate>("Employe/updatePrivate", employePrivate);
            modal.HideAsync();
            StateHasChanged();
        }
        public void EditAdresse(Adresse adresse)
        {
            Adresse = adresse;
            HttpClient.PatchAsJsonAsync<Adresse>("Employe/updateAdresse", adresse);
            StateHasChanged();
        }

        private Modal modal = default!;

        private async Task ShowEditPrivate()
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("EmployePrivate", EmployePrivate.Clone());
            parameters.Add("Adresse", Adresse.Clone());
            parameters.Add("Pays", Pays);
            parameters.Add("EditEmployePrivateEvent", EventCallback.Factory.Create<EmployePrivate>(this, EditPrivate));
            parameters.Add("EditAdresseEvent", EventCallback.Factory.Create<Adresse>(this, EditAdresse));
            await modal.ShowAsync<EditPrivate>(title: "Edition des infos privées", parameters: parameters);
        }
    }
}