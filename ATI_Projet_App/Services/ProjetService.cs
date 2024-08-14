using ATI_Projet_Models.Models;
using ATI_Projet_Models.Models.Projets;
using ATI_Projet_Tools.Services.Interfaces;
using System.Net.Http;

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

      public async Task<IEnumerable<Projet>> GotProjetBySociete(int id)
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<Projet>>("Projet/ByClient/" + id);
      }

      public async Task<IEnumerable<ProjetBC14>> GotProjetsBC14()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<ProjetBC14>>("Projet/BC14");
      }

      public async Task<IEnumerable<StatutProjet>> GotStatuts()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<StatutProjet>>("Projet/Statut");
      }
   }
}
