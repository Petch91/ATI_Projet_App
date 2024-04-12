using ATI_Projet_Models;
using ATI_Projet_Models.Types;
using ATI_Projet_Models.VCA;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ATI_Projet_Components.Personnel
{
    public partial class InfoProf : ComponentBase
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        [Parameter]
        public EmployeProf EmployeProf { get; set; }
        [Parameter]
        public EventCallback ProfChanged { get; set; }

        private void EditProf( EmployeProf employeProf)
        {
            HttpClient.PatchAsJsonAsync<EmployeProf>("Employe/updateProf", employeProf);
            modal.HideAsync();
            ProfChanged.InvokeAsync();
            StateHasChanged();
        }

        private Modal modal = default!;

        private async Task ShowEditProf()
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("EmployeProf", EmployeProf);
            parameters.Add("EditProfEvent", EventCallback.Factory.Create<EmployeProf>(this, EditProf));
            await modal.ShowAsync<EditProf>(title: "Edition des infos professionnelles", parameters: parameters);
        }
    }
}