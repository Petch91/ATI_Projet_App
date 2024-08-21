using ATI_Projet_Models;
using ATI_Projet_Models.Models.Forms;
using ATI_Projet_Models.Types;
using ATI_Projet_Models.VCA;
using ATI_Projets_Models;

namespace ATI_Projet_Tools.Services.Interfaces
{
   public interface IPersonnel
   {
      Task<IEnumerable<EmployeList>> GotPersonnelList();
      Task EditPhoto(int id , string path);
      Task EditSignature(int id, string path);

      Task<IEnumerable<Fonction>> GotFonctions();
      Task<IEnumerable<FonctionVCA>> GotFonctionsVCA();
      Task<IEnumerable<StatusVCA>> GotStatusVCA();
      Task<IEnumerable<TypeContrat>> GotTypesContrat();
      Task<IEnumerable<TypeMO>> GotTypesMO();
      Task<IEnumerable<Email>> GotEmails(int id);
      Task<IEnumerable<Telephone>> GotPhones(int id);
      Task<EmployeProfil> GotProfil(int id);
      Task<EmployePrivate> GotPrivate(int id);
      Task<EmployeProf> GotProf(int id);

      Task AddEmploye(PersonneForm personne);

      Task CreateGeneric<T>(string url, T entity);
      Task EditGeneric<T>(string url, T entity);
      Task DeleteGeneric<T>(string url);

      Task<Adresse> GotAdresse(int id);
      Task<int> EditAdresse(Adresse adresse);
      Task EditPrivate(EmployePrivate employePrivate);
      Task EditProf(EmployeProf employeProf);
      Task EditProfil(EmployeProfil employeProfil);

   }
}
