using ATI_Projet_Components;
using ATI_Projet_Models;
using ATI_Projet_Models.Models;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;

namespace ATI_Projet_App.Components.Pages.Personnels
{
    public partial class Liste 
    {
        [Parameter]
        public string Id { get; set; }
        [Inject]
        private HttpClient _client { get; set; }
        [Inject]
        private NavigationManager navigation { get; set; }

        List<Personnel> Personnel { get; set; }
        List<string> ExcluCol { get; set; } = new List<string> { "Titre" };
        Personnel person;
        bool OpenCanvas = false;
        private Offcanvas offcanvas = default!;
        protected async override Task OnInitializedAsync()
        {
            Personnel = await _client.GetFromJsonAsync<List<Personnel>>("Personnel/");
        }
        protected async override Task OnParametersSetAsync()
        {
            if(!string.IsNullOrEmpty(Id))
            {
                person = await _client.GetFromJsonAsync<Personnel>("Personnel/" + Id) ?? new Personnel();
                OpenCanvas = true;
            }
        }
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if(!firstRender)
            {
                if(person != null && OpenCanvas) await offcanvas.ShowAsync();
            }
        }
        private async Task OnHideOffcanvasClick()
        {
            OpenCanvas = false;
            await offcanvas.HideAsync();
            //navigation.NavigateTo("Personnel/",true);
        }
    }
}