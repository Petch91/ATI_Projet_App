using ATI_Projet_Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Reflection;
using System.Web;

namespace ATI_Projet_Components.Emails
{
    public partial class EmailForm
    {
        [Parameter]
        public Email Email { get; set; }
        [Parameter]
        public EventCallback<Email> OnValidation { get; set; }
        [Parameter] public string Error { get; set; }

        private List<string> descriptions = new List<string> { HttpUtility.HtmlEncode("Priv�e"), "Professionnelle" };
       
        public void IsValided()
        {
                OnValidation.InvokeAsync(Email);               

        }

    }
}