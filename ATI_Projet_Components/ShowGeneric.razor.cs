using Blazorise;
using Microsoft.AspNetCore.Components;

namespace ATI_Projet_Components
{
    public partial class ShowGeneric<TItem> : ComponentBase where TItem : class
    {
        [Parameter]
        public TItem Item { get; set; }

        [Parameter]
        public List<string> ExcludedProp { get; set; }

        protected override void OnParametersSet()
        {
            if (ExcludedProp == null)
            {
                ExcludedProp = new List<string>();
            }
        }
    }
}