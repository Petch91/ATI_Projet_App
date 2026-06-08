using ATI_Projet_Models.Models;
using ATI_Projet_Models.Models.Projets;
using ATI_Projet_Tools.Services.Interfaces;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using Microsoft.Graph;
using ATI_Projet_Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ATI_Projet_App.Services
{
   public class ProjetService(IHttpClientFactory httpClientFactory) : IProjet
   {
      private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

      public async Task<IEnumerable<AxeMarche>> GotAxeMarche()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<AxeMarche>>("AxeMarche");
      }

      // Fiches BC14 actives (Status != Completed) pour la page Synchro des Responsables.
      // L'endpoint FicheBc14 renvoie TOUS les jobs → on filtre les clôturés côté Blazor.
      public async Task<IEnumerable<FicheBC14>> GotFichesBc14()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<FicheBC14>>("Projet/FicheBc14Actives");
      
      }

      // Tous les jobs BC14 (actifs + clôturés) depuis la table locale ATI, pour CompStatut.
      public async Task<IEnumerable<FicheBC14>> GotAllFichesBc14()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<FicheBC14>>("Projet/FicheBc14");
      }

      // Ressources BC14 lues depuis la table locale ATI synchronisée en arrière-plan.
      public async Task<IEnumerable<RessourceBC14>> GotRessourcesBc14()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<RessourceBC14>>("Projet/RessourceBc14");
      }

      public async Task<IEnumerable<Projet>> GotProjetBySociete(int id)
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<Projet>>("Projet/ByClient/" + id);
      }

      public async Task<IEnumerable<ProjetBC14>> GotProjetsBc14()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<ProjetBC14>>("Projet/BC14");
      }

      public async Task<IEnumerable<ProjetBC14>> GotProjetsCompStatut()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<ProjetBC14>>("Projet/CompStatut");
      }

      public async Task<IEnumerable<StatutProjet>> GotStatuts()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<StatutProjet>>("Projet/Statut");
      }

      public async Task<bool> PatchFicheBC14(string no, string person)
      {
         using var client = _httpClientFactory.CreateClient("BC14");
         //var username = "ATI_WS";
         //var password = "U4VsMoxs4179tL7VDgkBcoffrAJVKVibmEoopIbPelw=";

         ////Encodez les informations d'identification en Base64
         //var authToken = Encoding.ASCII.GetBytes($"{username}:{password}");
         //var authHeaderValue = Convert.ToBase64String(authToken);

         //// Ajoutez l'en-tête d'autorisation
         //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
         client.DefaultRequestHeaders.Add("If-Match", "*");

         // Préparez le contenu JSON à envoyer
         string jsonToSend = JsonConvert.SerializeObject(new { Person_Responsible = person });
         var body = new StringContent(jsonToSend, Encoding.UTF8, "application/json");


         try
         {
            // Envoi de la requête PATCH
            var response = await client.PatchAsync($"Job(No='{no}')", body);

            // Vérifiez le statut de la réponse
            return response.IsSuccessStatusCode;
         }
         catch (Exception ex)
         {
            // Loggez l'erreur ou gérez-la selon vos besoins
            Console.WriteLine(ex.Message);
            return false;
         }
      }
   }
}
public class ODataResponse<T>
{
   [JsonPropertyName("value")]
   public List<T> Value { get; set; }
}