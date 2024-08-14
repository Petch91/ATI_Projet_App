using ATI_Projet_Models;
using ATI_Projet_Models.Models.Societes.Clients;
using ATI_Projet_Tools.Services.Interfaces;

namespace ATI_Projet_App.Services
{
   public class CommonService(IHttpClientFactory httpClientFactory) : ICommon
   {
      private readonly IHttpClientFactory httpClientFactory = httpClientFactory;

      public async Task<Adresse> GetAdresse(int id)
      {
         using var client = httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<Adresse>("Adresse/" + id) ?? new Adresse();
      }

      public async Task<IEnumerable<DeptSimplifiedList>> GetDepts()
      {
         using var client = httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<DeptSimplifiedList>>("departement/simplifiedList");
      }

      public async Task<Dictionary<string, string>> GetCountrys(string codeLangue)
      {
         using var client = httpClientFactory.CreateClient();
         return await client.GetFromJsonAsync<Dictionary<string, string>>("https://www.iso-country-code.com/api/?lang=" + codeLangue + "&output=json") ?? new Dictionary<string, string>();
      }

   }
}
