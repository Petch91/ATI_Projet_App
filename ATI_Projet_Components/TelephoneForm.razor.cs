using ATI_Projet_Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

namespace ATI_Projet_Components
{
    public partial class TelephoneForm
    {
        [Parameter]
        public Telephone Telephone { get; set; }
        [Parameter]
        public EventCallback<bool> OnValidation { get; set; }

        [Parameter]
        public bool IsValid { get; set; } = false;
        [Parameter]
        public EventCallback<Telephone> TelephoneChanged { get; set; }
        [Parameter]
        public EventCallback<bool> IsValidChanged { get; set; }
        [Parameter]
        public EventCallback<Telephone> GoEditTelephone { get; set; }

        private EditForm EditForm { get; set; } = new EditForm();

        private bool IsChanged { get; set; } = false;

        //protected override void OnAfterRender(bool firstRender)
        //{
        //    if(firstRender) Submit();
        //}

        public void Submit()
        {
            if (EditForm.EditContext.Validate()) EditForm.OnValidSubmit.InvokeAsync();
            else EditForm.OnInvalidSubmit.InvokeAsync();
            TelephoneChanged.InvokeAsync(Telephone);
        }
        public void IsValided()
        {
            if (IsChanged)
            {
                IsValid = true;
                IsValidChanged.InvokeAsync(IsValid);
                OnValidation.InvokeAsync(IsValid);
            }
        }
        public void IsNotValided()
        {
            IsValid = false;
            IsValidChanged.InvokeAsync(IsValid);
            OnValidation.InvokeAsync(IsValid);
        }

        public void GoEdit()
        {
            if (EditForm.EditContext.Validate()) GoEditTelephone.InvokeAsync(Telephone);
        }

        public void Changed()
        {
            IsChanged = true;
        }
    }
}