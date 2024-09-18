using ATI_Projet_Cultures.Tools;
using ATI_Projet_Models;
using ATI_Projet_Models.Types;
using ATI_Projet_Models.VCA;
using Microsoft.AspNetCore.Components;

namespace ATI_Projet_Components.Personnel
{
   public partial class EditProfil : ComponentBase, IDisposable
   {
      [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

      [Parameter]
      public EmployeProfil EmployeProfil { get; set; }
      [Parameter]
      public IEnumerable<DeptSimplifiedList> departements { get; set; }
      [Parameter]
      public IEnumerable<Fonction> fonctions { get; set; }
      [Parameter]
      public IEnumerable<FonctionVCA> fonctionsVCA { get; set; }
      [Parameter]
      public IEnumerable<StatusVCA> statusVCA { get; set; }
      [Parameter]
      public IEnumerable<TypeContrat> TypesContrat { get; set; }

      [Parameter]
      public IEnumerable<TypeMO> TypesMO { get; set; }
      [Parameter]
      public EventCallback<EmployeProfil> EditProfilEvent { get; set; }

      private List<string> ChoixTitre = new List<string> { "", "M.", "Mme", "X." };


      protected override void OnInitialized() => LanguageNotifier.SubscribeLanguageChange(this);
      public void Dispose() => LanguageNotifier.UnsubscribeLanguageChange(this);

      public void Edit()
      {
         EditProfilEvent.InvokeAsync((EmployeProfil)EmployeProfil.Clone());

      }
   }
}