using ATI_Projet_Models.Models;
using ATI_Projet_Models.Models.Projets;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Tools.Services.Interfaces
{
   public interface IProjet
   {
      Task<IEnumerable<Projet>> GotProjetBySociete(int id);

      Task<IEnumerable<ProjetBC14>> GotProjetsBc14();

      Task<IEnumerable<StatutProjet>> GotStatuts();

      Task<IEnumerable<AxeMarche>> GotAxeMarche();
      
      Task<IEnumerable<FicheBC14>> GotFichesBc14();

      Task<bool> PatchFicheBC14(string no, string person);
      Task<IEnumerable<RessourceBC14>> GotRessourcesBc14();
   }
}
