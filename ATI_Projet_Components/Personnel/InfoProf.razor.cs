using ATI_Projet_Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;

namespace ATI_Projet_Components.Personnel
{
    public partial class InfoProf : ComponentBase
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        [Parameter]
        public EmployeProf EmployeProf { get; set; }

        private void EditProf()
        {
            HttpClient.PatchAsJsonAsync<EmployeProf>("Employe/updateProf", EmployeProf);
        }
    }
}