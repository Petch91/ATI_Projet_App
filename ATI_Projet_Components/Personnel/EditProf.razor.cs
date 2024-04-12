using ATI_Projet_Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ATI_Projet_Components.Personnel
{
    public partial class EditProf : ComponentBase
    {
        [Parameter]
        public EmployeProf EmployeProf { get; set; }
        [Parameter]
        public EventCallback<EmployeProf> EditProfEvent { get; set; }
        public void Edit()
        {
            EditProfEvent.InvokeAsync(EmployeProf);

        }
    }
}