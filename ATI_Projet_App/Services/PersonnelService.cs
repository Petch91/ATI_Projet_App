using ATI_Projet_Models;
using ATI_Projet_Models.Models.Forms;
using ATI_Projet_Models.Models.Projets;
using ATI_Projet_Models.Types;
using ATI_Projet_Models.VCA;
using ATI_Projet_Tools.Services.Interfaces;
using ATI_Projets_Models;
using Microsoft.Graph.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace ATI_Projet_App.Services
{
   public class PersonnelService(IHttpClientFactory httpClientFactory) : IPersonnel
   {
      private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

      public async Task AddEmploye(PersonneForm personne)
      {
         using var client = _httpClientFactory.CreateClient("api");
         await client.PostAsJsonAsync<PersonneForm>("Employe/", personne);
      }

      public async Task EditPhoto(int id, string path)
      {
         using var client = _httpClientFactory.CreateClient("api");
         await client.PatchAsJsonAsync("Employe/changePhoto", new { Id = id, Path = path });
      }

      public async Task EditSignature(int id, string path)
      {
         using var client = _httpClientFactory.CreateClient("api");
         await client.PatchAsJsonAsync("Employe/changeSignature", new { Id = id, Path = path });
      }

      public async Task<IEnumerable<Email>> GotEmails(int id)
      {
         using var client = httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<Email>>("Email/ByEmploye/" + id);
      }

      public async Task<IEnumerable<Telephone>> GotPhones(int id)
      {
         using var client = httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<Telephone>>("Telephone/ByEmploye/" + id);
      }

      public async Task<IEnumerable<FonctionVCA>> GotFonctionsVCA()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<FonctionVCA>>("VCA/Fonction");
      }

      public async Task<IEnumerable<Fonction>> GotFonctions()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<Fonction>>("Fonction");
      }

      public async Task<IEnumerable<EmployeList>> GotPersonnelList()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<EmployeList>>("Employe");
      }


      public async Task<EmployePrivate> GotPrivate(int id)
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<EmployePrivate>("Employe/private/" + id) ?? new EmployePrivate();
      }

      public async Task<EmployeProf> GotProf(int id)
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<EmployeProf>("Employe/prof/" + id) ?? new EmployeProf();
      }

      public async Task<EmployeProfil> GotProfil(int id)
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<EmployeProfil>("Employe/profil/" + id) ?? new EmployeProfil();
      }

      public async Task<IEnumerable<StatusVCA>> GotStatusVCA()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<StatusVCA>>("VCA/Status");
      }

      public async Task<IEnumerable<TypeContrat>> GotTypesContrat()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<TypeContrat>>("type/contrat");
      }

      public async Task<IEnumerable<TypeMO>> GotTypesMO()
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<IEnumerable<TypeMO>>("type/MO");
      }

      public async Task CreateGeneric<T>(string url, T entity)
      {
         using var client = _httpClientFactory.CreateClient("api");
         await client.PostAsJsonAsync<T>(url, entity);
      }

      public async Task EditGeneric<T>(string url, T entity)
      {
         using var client = _httpClientFactory.CreateClient("api");
         await client.PatchAsJsonAsync<T>(url, entity);
      }

      public async Task DeleteGeneric<T>(string url)
      {
         using var client = _httpClientFactory.CreateClient("api");
         await client.DeleteFromJsonAsync<T>(url);
      }

      public async Task<Adresse> GotAdresse(int id)
      {
         using var client = _httpClientFactory.CreateClient("api");
         return await client.GetFromJsonAsync<Adresse>("Employe/adresse/" + id);
      }

      public async Task<int> EditAdresse(Adresse adresse)
      {
         using var client = _httpClientFactory.CreateClient("api");
         var reponse = client.PatchAsJsonAsync<Adresse>("Employe/updateAdresse", adresse);
         return await reponse.Result.Content.ReadFromJsonAsync<int>();
      }

      public async Task EditPrivate(EmployePrivate employePrivate)
      {
         using var client = _httpClientFactory.CreateClient("api");
         await client.PatchAsJsonAsync<EmployePrivate>("Employe/updatePrivate", employePrivate);
      }

      public async Task EditProf(EmployeProf employeProf)
      {
         using var client = _httpClientFactory.CreateClient("api");
         await client.PatchAsJsonAsync<EmployeProf>("Employe/updateProf", employeProf);
      }

      public async Task EditProfil(EmployeProfil employeProfil)
      {
         using var client = _httpClientFactory.CreateClient("api");
         await client.PatchAsJsonAsync<EmployeProfil>("Employe/updateProfil", employeProfil);
      }
   }
}
