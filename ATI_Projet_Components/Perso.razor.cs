using ATI_Projet_Models;
using ATI_Projet_Tools;
using Microsoft.AspNetCore.Components;

namespace ATI_Projet_Components
{
    public partial class Perso : ComponentBase
    {
        [Inject]
        private  ApiRequester api {  get; set; }
        [Parameter]
        public Personnel Utilisateur { get; set; } = new Personnel();

        private List<string> ChoixTitre = new List<string> { "","M.", "Mme", "X." };

        private bool IsEditable = false;

        public void ChangeMode()
        {
            IsEditable = !IsEditable;
            StateHasChanged();
        }

        public void EditUser()
        {
            api.Patch<Personnel>(Utilisateur, "Personnel/update/" + Utilisateur.Id);
        }


    }
}