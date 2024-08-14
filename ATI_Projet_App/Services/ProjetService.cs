using ATI_Projet_Models.Models;
using ATI_Projet_Models.Models.Projets;
using ATI_Projet_Tools.Services.Interfaces;
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

      public async Task<IEnumerable<FicheBC14>> GotFichesBc14()
      {
         using var client = _httpClientFactory.CreateClient("BC14");
         //var username = "ATI_WS";
         //var password = "U4VsMoxs4179tL7VDgkBcoffrAJVKVibmEoopIbPelw=";
    
         //// Encodez les informations d'identification en Base64
         //var authToken = Encoding.ASCII.GetBytes($"{username}:{password}");
         //var authHeaderValue = Convert.ToBase64String(authToken);

         //// Ajoutez l'en-tête d'autorisation
         //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

         var reponse  = await client.GetAsync("Fiches_ATI?$filter=Status ne 'Completed'&$select=No,Description,Person_Responsible,Responsable_Nom");
         if (reponse.IsSuccessStatusCode)
         {
            var jsonResponse =  await reponse.Content.ReadAsStringAsync();
            // Désérialiser la réponse JSON en une liste d'objets FicheBC14
            var options = new JsonSerializerOptions
            {
               PropertyNameCaseInsensitive = true
            };

            var ficheBC14List = System.Text.Json.JsonSerializer.Deserialize<ODataResponse<FicheBC14>>(jsonResponse, options);
            return ficheBC14List.Value;

         }

         return new List<FicheBC14>();
      }

      public async Task<IEnumerable<RessourceBC14>> GotRessourcesBc14()
      {
         using var client = _httpClientFactory.CreateClient("BC14");

         var reponse = await client.GetAsync("Ressources_ATI?$select=No,Name");
         if (reponse.IsSuccessStatusCode)
         {
            var jsonResponse = await reponse.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
               PropertyNameCaseInsensitive = true
            };

            var ressourceBC14List = System.Text.Json.JsonSerializer.Deserialize<ODataResponse<RessourceBC14>>(jsonResponse, options);
            return ressourceBC14List.Value;

         }

         return new List<RessourceBC14>();
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
            var response = await client.PatchAsync($"Fiches_ATI(No='{no}')", body);

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