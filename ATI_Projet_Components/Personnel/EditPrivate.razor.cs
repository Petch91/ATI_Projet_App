using ATI_Projet_Models;
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
        public EventCallback<Adresse> EditAdresseEvent{ get; set; }
        [Parameter]
        public EventCallback<EmployePrivate> EditEmployePrivateEvent { get; set; }

 
            
        

        public async void EditEmployePrivate()
        {
            await EditAdresseEvent.InvokeAsync(Adresse);
            EditEmployePrivateEvent.InvokeAsync(EmployePrivate);
        }

    }
}