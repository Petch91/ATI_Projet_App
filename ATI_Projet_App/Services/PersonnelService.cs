using ATI_Projet_Models.Models.Projets;
using ATI_Projet_Tools.Services.Interfaces;
using ATI_Projets_Models;

namespace ATI_Projet_App.Services
{
   public class PersonnelService(IHttpClientFactory httpClientFactory) : IPersonnel
   {
      private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

      public async Task<IEnumerable<EmployeList>> GotPersonnelList()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<EmployeList>>("Employe");
      }
   }
}
