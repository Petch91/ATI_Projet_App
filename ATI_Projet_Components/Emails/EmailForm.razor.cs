using ATI_Projet_Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Reflection;

namespace ATI_Projet_Components.Emails
{
    public partial class EmailForm
    {
        [Parameter]
        public Email Email { get; set; }
        [Parameter]
        public EventCallback<Email> OnValidation { get; set; }

        private List<string> descriptions = new List<string> { "Privée", "Professionnelle" };
       
        public void IsValided()
        {
                OnValidation.InvokeAsync(Email);               

        }

    }
}