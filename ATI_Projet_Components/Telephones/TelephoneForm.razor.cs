using ATI_Projet_Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

namespace ATI_Projet_Components.Telephones
{
    public partial class TelephoneForm
    {
        [Parameter]
        public Telephone Telephone { get; set; }
        [Parameter]
        public EventCallback<Telephone> OnValidation { get; set; }

        private List<string> descriptions = new List<string>{ @"Fixe Privé","Fixe Professionnel", @"Gsm Privé", "Gsm Professionnel" };
        
        public void IsValided()
        {

                OnValidation.InvokeAsync(Telephone);
            
        }

    }
}