using ATI_Projet_Models;
using ATI_Projet_Models.Models;
using ATI_Projet_Models.Models.Societes.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Tools.Services.Interfaces
{
   public interface ISociete
   {
      Task<IEnumerable<AxeMarche>> GotAxeMarche();
      Task<IEnumerable<ClientList>> GotClientList();
      Task<IEnumerable<Email>> GotEmails(int id);
      Task<IEnumerable<Telephone>> GotPhones(int id);

      Task<ClientInfo> GotClientInfo(int id);
      Task<ClientSigna> GotClientSigna(int id);
   }
}
