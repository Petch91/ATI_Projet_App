using ATI_Projet_Models.Models.Forms;
using Microsoft.AspNetCore.Components;

namespace ATI_Projet_Components
{
    public partial class CreatePersonne : ComponentBase
    {
        [Parameter] public EventCallback<PersonneForm> CreateEvent { get; set; }

        private PersonneForm form = new PersonneForm();

      [Parameter]
      public Dictionary<string, string> Langues { get; set; }

      [Parameter]
      public Dictionary<string, string> Nationalites { get; set; }

      private List<string> ChoixTitre = new List<string> { "M.", "Mme", "X." };

        private async Task Submit()
        {
            await CreateEvent.InvokeAsync(form);
            form = new PersonneForm();
            StateHasChanged();
        }
    }
}