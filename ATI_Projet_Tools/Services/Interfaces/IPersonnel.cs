using ATI_Projets_Models;

namespace ATI_Projet_Tools.Services.Interfaces
{
   public interface IPersonnel
   {
      Task<IEnumerable<EmployeList>> GotPersonnelList();
   }
}
