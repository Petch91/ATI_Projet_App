using ATI_Projet_Components.Personnel;
using ATI_Projet_Cultures.Tools;
using ATI_Projet_Models;
using ATI_Projet_Tools.Services.Interfaces;
using BlazorBootstrapPerso;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Runtime.InteropServices;

namespace ATI_Projet_Components.Emails
{
   public partial class EmailList : IDisposable
   {
      [Inject] private ICommon common { get; set; }
      [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

      [Parameter]
      public List<Email> emails { get; set; }
      [Parameter]
      public int PersonneId { get; set; } = 0;
      [Parameter]
      public int SocieteId { get; set; } = 0;
      [Parameter]
      public EventCallback<List<Email>> emailsChanged { get; set; }

      private string ErrorMessage = string.Empty;


      private Modal modal = default!;


      List<ToastMessage> messages = new List<ToastMessage>();

      protected override void OnInitialized() => LanguageNotifier.SubscribeLanguageChange(this);
      public void Dispose() => LanguageNotifier.UnsubscribeLanguageChange(this);


      private async Task ShowEdit(object id)
      {
         var parameters = new Dictionary<string, object>();
         parameters.Add("Email", emails.FirstOrDefault(e => e.Id == (int)id).Clone());
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
         parameters.Add("Error", ErrorMessage);
         parameters.Add("OnValidation", EventCallback.Factory.Create<Email>(this, Edit));
         await modal.ShowAsync<EmailForm>(title: "Ajout d'une adresse Mail", parameters: parameters);
      }

      public async Task Edit(Email email)
      {
         if (email.Id > 0) emails[emails.IndexOf(emails.First(e => e.Id == email.Id))] = email;
         var result = common.EditEmail(email);
         if (!result.Result.IsSuccessStatusCode)
         {
            var body = await result.Result.Content.ReadAsStringAsync();
            ErrorMessage = body.Contains("unique") ? @"Attention l'email existe déjà" : result.Result.ReasonPhrase;
            messages.Add(new ToastMessage { Type = ToastType.Danger, Title = "Erreur en DB", HelpText = $"{DateTime.UtcNow}", Message = ErrorMessage });
         }
         else
         {
            ErrorMessage = "";
            await modal.HideAsync();
            await emailsChanged.InvokeAsync(emails);
         }
         StateHasChanged();
      }
      public async Task Delete(object id)
      {
         common.DeleteEmail("Email?id=" + id);
         emails.Remove(emails.First(e => e.Id == (int)id));
         await Task.Delay(500);
         await emailsChanged.InvokeAsync(emails);
         StateHasChanged();
      }
   }
}