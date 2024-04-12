using ATI_Projet_Components.Personnel;
using ATI_Projet_Models;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace ATI_Projet_Components.Emails
{
    public partial class EmailList
    {
        [Inject]
        private HttpClient httpClient { get; set; } = default!;
        [Parameter]
        public List<Email> emails { get; set; }
        [Parameter]
        public int PersonneId { get; set; } = 0;
        [Parameter]
        public int SocieteId { get; set; } = 0;
        [Parameter]
        public EventCallback<List<Email>> emailsChanged { get; set; }


        private Modal modal = default!;

        private async Task ShowEdit(int id)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Email", emails.FirstOrDefault(e => e.Id == id).Clone());
            parameters.Add("OnValidation", EventCallback.Factory.Create<Email>(this, Edit));
            await modal.ShowAsync<EmailForm>(title: "Edition Mail", parameters: parameters);
        }
        private async Task ShowCreateMail()
        {
            Email e = new Email();
            e.PersonneId = PersonneId;
            e.SocieteId = SocieteId;
            var parameters = new Dictionary<string, object>();
            parameters.Add("Email", e);
            parameters.Add("OnValidation", EventCallback.Factory.Create<Email>(this, Edit));
            await modal.ShowAsync<EmailForm>(title: "Ajout d'une adresse Mail", parameters: parameters);
        }

        public async void Edit(Email email)
        {
            if(email.Id >0) emails[emails.IndexOf(emails.First(e => e.Id == email.Id))] = email;
            await httpClient.PutAsJsonAsync<Email>("Employe/updateEmail", email);
            modal.HideAsync();
            emailsChanged.InvokeAsync(emails);
            StateHasChanged();

        }
        public async void Delete(int id)
        {
            httpClient.DeleteFromJsonAsync<Email>("Email?id=" + id);
            emails.Remove(emails.First(e => e.Id == id));
            await Task.Delay(500);
            await emailsChanged.InvokeAsync(emails);
            StateHasChanged();
        }
    }
}