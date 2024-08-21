using ATI_Projet_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Tools.Services.Interfaces
{
   public interface ICommon
   {
      Task<Adresse> GetAdresse(int id);
      Task<Dictionary<string, string>> GetCountrys(string codeLangue);
      Task<IEnumerable<DeptSimplifiedList>> GetDepts();
      Task<HttpResponseMessage> EditEmail(Email email);
      Task DeleteEmail(string url);
      Task<HttpResponseMessage> EditTelephone(Telephone telephone);
      Task DeleteTelephone(string url);
   }
}
