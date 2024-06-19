using ATI_Projet_Models;
using ATI_Projet_Models.Events;
using Microsoft.AspNetCore.Components;

namespace ATI_Projet_Components.Personnel
{
    public partial class EditPrivate
    {
        [Parameter]
        public EmployePrivate EmployePrivate { get; set; }
        [Parameter]
        public Adresse Adresse { get; set; }

        [Parameter]
        public Dictionary<string, string> Pays { get; set; }

        [Parameter]
        public EventCallback<EditPrivateArgs> EditEmployePrivateEvent { get; set; }

 
            
        

        public async Task EditEmployePrivate()
        {
            await EditEmployePrivateEvent.InvokeAsync(new EditPrivateArgs { Employe = EmployePrivate, Adresse = Adresse});
        }

    }
}