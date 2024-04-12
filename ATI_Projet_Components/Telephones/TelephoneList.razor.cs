using ATI_Projet_Models;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace ATI_Projet_Components.Telephones
{
    public partial class TelephoneList
    {
        [Inject]
        private HttpClient httpClient { get; set; } = default!;
        [Parameter]
        public List<Telephone> telephones { get; set; }
        [Parameter]
        public int PersonneId { get; set; } = 0;
        [Parameter]
        public int SocieteId { get; set; } = 0;
        [Parameter]
        public EventCallback<List<Telephone>> telephonesChanged { get; set; }


        private Modal modal = default!;

        private async Task ShowEdit(int id)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Telephone", telephones.FirstOrDefault(e => e.Id == id).Clone());
            parameters.Add("OnValidation", EventCallback.Factory.Create<Telephone>(this, Edit));
            await modal.ShowAsync<TelephoneForm>(title: "Edition Mail", parameters: parameters);
        }
        private async Task ShowCreateTelephone()
        {
            Telephone t = new Telephone();
            t.PersonneId = PersonneId;
            t.SocieteId = SocieteId;
            var parameters = new Dictionary<string, object>();
            parameters.Add("Telephone", t);
            parameters.Add("OnValidation", EventCallback.Factory.Create<Telephone>(this, Edit));
            await modal.ShowAsync<TelephoneForm>(title: "Ajout d'une adresse Mail", parameters: parameters);
        }

        public async void Edit(Telephone telephone)
        {
            if (telephone.Id > 0) telephones[telephones.IndexOf(telephones.First(t => t.Id == telephone.Id))] = telephone;
            await httpClient.PutAsJsonAsync<Telephone>("Employe/updateTelephone", telephone);
            modal.HideAsync();
            telephonesChanged.InvokeAsync(telephones);
            StateHasChanged();

        }
        public async void Delete(int id)
        {
            httpClient.DeleteFromJsonAsync<Telephone>("Telephone?id=" + id);
            telephones.Remove(telephones.First(e => e.Id == id));
            await Task.Delay(500);
            await telephonesChanged.InvokeAsync(telephones);
            StateHasChanged();
        }
    }
}