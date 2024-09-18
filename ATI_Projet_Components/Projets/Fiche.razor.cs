using ATI_Projet_Cultures.Locales;
using ATI_Projet_Models.Models.Projets;
using ATI_Projet_Models.Models.Societes.Clients;
using ATI_Projet_Models.Models;
using ATI_Projet_Models;
using ATI_Projet_Tools.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Net.Http;
using BlazorBootstrapPerso;
using System.Globalization;
using ATI_Projets_Models;
using ATI_Projet_Cultures.Tools;

namespace ATI_Projet_Components.Projets
{
   public partial class Fiche : ComponentBase, IDisposable
   {

      [Inject] private IProjet Projet { get; set; }
      [Inject] private ICommon common { get; set; }
      [Inject] private IPersonnel personnel { get; set; }

      [Inject] private IStringLocalizer<PersonnelResource> localizer { get; set; }
      [Inject] private IStringLocalizer<PaysResource> paysLocalizer { get; set; }
      [Inject] private LanguageChangeNotifier LanguageNotifier { get; set; }

      [Parameter] public EventCallback<int> ClientChanged { get; set; }

      [Parameter] public int ClientId { get; set; }

      public List<Projet> ProjetsClient { get; set; }

      private int Id;
      private int Code;
      private Projet projet;


      private IEnumerable<AxeMarche> axesMarche;
      private IEnumerable<DeptSimplifiedList> departements;
      private IEnumerable<StatutProjet> statuts;
      private IEnumerable<EmployeList> personnelList;

      private Offcanvas offcanvas = default!;

      private Adresse Adresse { get; set; }

      private string CodePays = "be";
      private Dictionary<string, string> Pays { get; set; }


      protected async override Task OnInitializedAsync()
      {
         LanguageNotifier.SubscribeLanguageChange(this);
         axesMarche = new List<AxeMarche>();
         axesMarche = await Projet.GotAxeMarche();
         departements = new List<DeptSimplifiedList>();
         departements = await common.GetDepts();
         statuts = new List<StatutProjet>();
         statuts = await Projet.GotStatuts();
         personnelList = new List<EmployeList>();
         personnelList = await personnel.GotPersonnelList();

         StateHasChanged();
      }

      public void Dispose() => LanguageNotifier.UnsubscribeLanguageChange(this);

      protected async override Task OnParametersSetAsync()
      {
         var temp = await Projet.GotProjetBySociete(ClientId);
         ProjetsClient = new List<Projet>();
         ProjetsClient = temp.ToList();

         if (ProjetsClient != null && ProjetsClient.Count() > 0)
         {
            projet = ProjetsClient.First();
            Id = projet.Id;
            Code = projet.Code;

         }
         Adresse = new Adresse();
         if (projet != null)
         {
            Adresse = await common.GetAdresse(projet.AdresseId);
            CodePays = Adresse.Pays;
         }
         StateHasChanged();
      }
      private async Task SelectChange(int id)
      {
         if (ProjetsClient.Select(x => x.Code).Contains(id))
         {
            Code = id;

            projet = ProjetsClient.First(x => x.Code == id);
            Id = projet.Id;
            await ClientChanged.InvokeAsync(Id);
            StateHasChanged();
            await offcanvas.HideAsync();
         }

      }


      private async Task ChangeProjet()
      {

      }

      public async Task ShowCanvas()
      {
         await offcanvas.ShowAsync();
      }
      public async Task OnHideOffcanvasClick()
      {
         await offcanvas.HideAsync();
      }
   }
}