using ATI_Projet_Models;
using ATI_Projet_Models.Models.Societes.Clients;
using ATI_Projet_Tools.Services.Interfaces;
using System.Net.Http;

namespace ATI_Projet_App.Services
{
   public class CommonService(IHttpClientFactory httpClientFactory) : ICommon
   {
      private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

      public async Task<Adresse> GetAdresse(int id)
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<Adresse>("Adresse/" + id) ?? new Adresse();
      }

      public async Task<IEnumerable<DeptSimplifiedList>> GetDepts()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<DeptSimplifiedList>>("departement/simplifiedList");
      }

      public async Task<Dictionary<string, string>> GetCountrys(string codeLangue)
      {
         using var client = _httpClientFactory.CreateClient();
         return await client.GetFromJsonAsync<Dictionary<string, string>>("https://www.iso-country-code.com/api/?lang=" + codeLangue + "&output=json") ?? new Dictionary<string, string>();
      }

      public async Task<HttpResponseMessage> EditEmail(Email email)
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.PutAsJsonAsync<Email>("Employe/updateEmail", email);
      }

      public async Task DeleteEmail(string url)
      {
         using var client = _httpClientFactory.CreateClient("api");
         await client.DeleteAsync(url);
      }

      public async Task<HttpResponseMessage> EditTelephone(Telephone telephone)
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.PutAsJsonAsync<Telephone>("Employe/updateTelephone", telephone);
      }

      public async Task DeleteTelephone(string url)
      {
         using var client = _httpClientFactory.CreateClient("api");
         await client.DeleteAsync(url);
      }
   }
}
