using ATI_Projet_Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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
        [Parameter]
        public EventCallback<Adresse> adressChanged { get; set; }

        [Parameter]
        public Dictionary<string,string> Pays { get; set; }

        private EditForm EditForm { get; set; } = new EditForm();


        public void Changed()
        {
            if (EditForm.EditContext.Validate())
            {
                adressChanged.InvokeAsync(adress);
                //GoEditAdresse.InvokeAsync(adress);
            }

        }

        public void EditAdresse()
        {
            GoEditAdresse.InvokeAsync(adress);
        }
    }
}