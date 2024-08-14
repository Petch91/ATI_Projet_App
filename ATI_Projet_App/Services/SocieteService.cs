using ATI_Projet_Models;
using ATI_Projet_Models.Models;
using ATI_Projet_Models.Models.Societes.Clients;
using ATI_Projet_Tools.Services.Interfaces;

namespace ATI_Projet_App.Services
{
   public class SocieteService(IHttpClientFactory httpClientFactory) : ISociete
   {
      private readonly IHttpClientFactory httpClientFactory = httpClientFactory;

      public async Task<IEnumerable<ClientList>> GotClientList()
      {
         using var client = httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<List<ClientList>>("Client/list/") ?? new List<ClientList>();
      }

      public async Task<IEnumerable<AxeMarche>> GotAxeMarche()
      {
         using var client = httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<AxeMarche>>("AxeMarche");
      }

      public async Task<IEnumerable<Email>> GotEmails(int id)
      {
         using var client = httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<Email>>("Email/BySociete/" + id);
      }

      public async Task<IEnumerable<Telephone>> GotPhones(int id)
      {
         using var client = httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<Telephone>>("Telephone/BySociete/" + id);
      }

      public async Task<ClientInfo> GotClientInfo(int id)
      {
         using var client = httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<ClientInfo>("Client/info/" + id) ?? new ClientInfo();
      }

      public async Task<ClientSigna> GotClientSigna(int id)
      {
         using var client = httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<ClientSigna>("Client/signa/" + id) ?? new ClientSigna();
      }
   }
}
