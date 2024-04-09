using ATI_Projet_Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;

namespace ATI_Projet_Components
{
    public partial class AdresseComponent : ComponentBase
    {
        [Parameter]
        public Adresse adress { get; set; }
        [Parameter]
        public EventCallback<Adresse> GoEditAdresse { get; set; }

        public void EditAdresse()
        {
            GoEditAdresse.InvokeAsync(adress);
        }
    }
}