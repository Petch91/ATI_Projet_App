using ATI_Projet_Models;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph.Models;
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


        private string ErrorMessage = string.Empty;


        private Modal modal = default!;


        List<ToastMessage> messages = new List<ToastMessage>();

        private async Task ShowEdit(object id)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Telephone", telephones.FirstOrDefault(e => e.Id == (int)id).Clone());
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

        public async Task Edit(Telephone telephone)
        {
            if (telephone.Id > 0) telephones[telephones.IndexOf(telephones.First(t => t.Id == telephone.Id))] = telephone;
            using var result = httpClient.PutAsJsonAsync<Telephone>("Employe/updateTelephone", telephone);
            if (!result.Result.IsSuccessStatusCode)
            {
                var body = await result.Result.Content.ReadAsStringAsync();
                ErrorMessage = body.Contains("unique") ? @"Attention le Numéro existe déjà" : result.Result.ReasonPhrase;
                messages.Add(new ToastMessage { Type = ToastType.Danger, Title = "Erreur en DB", HelpText = $"{DateTime.Now}", Message = ErrorMessage });
            }
            else
            {
                await modal.HideAsync();
                await telephonesChanged.InvokeAsync(telephones);
            }
            StateHasChanged();

        }
        public async Task Delete(object id)
        {
            httpClient.DeleteFromJsonAsync<Telephone>("Telephone?id=" + id);
            telephones.Remove(telephones.First(e => e.Id == (int)id));
            await Task.Delay(500);
            await telephonesChanged.InvokeAsync(telephones);
            StateHasChanged();
        }
    }
}