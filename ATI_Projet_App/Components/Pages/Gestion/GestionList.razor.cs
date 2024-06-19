using ATI_Projet_Components;
using ATI_Projet_Models;
using ATI_Projet_Models.Types;
using ATI_Projet_Models.VCA;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.Security;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Localization;
using System.Globalization;
using ATI_Projet_Cultures.Locales;

namespace ATI_Projet_App.Components.Pages.Gestion
{
   public partial class GestionList : ComponentBase
   {
      [Inject] private HttpClient HttpClient { get; set; }
      [Inject] private IStringLocalizer<PersonnelResource> localizer { get; set; }

      private List<Type> Liste { get; set; } = new List<Type> { typeof(Fonction), typeof(FonctionVCA), typeof(StatusVCA), typeof(TypeContrat), typeof(TypeMO) };
      private List<object> items = new List<object>();

      private string type = "Fonction";

      private Modal modal { get; set; }
      private ConfirmDialog dialog;

      protected async override Task OnInitializedAsync()
      {
         List<Fonction> list = await HttpClient.GetFromJsonAsync<List<Fonction>>("fonction/");
         items = list.Cast<object>().ToList();
      }


      private async Task SelectChange(string classe)
      {
         type = classe;
         items = new List<object>();
         switch (classe)
         {
            case "Fonction":
               {
                  List<Fonction> list = await HttpClient.GetFromJsonAsync<List<Fonction>>("fonction/");
                  items = list.Cast<object>().ToList();
                  break;
               }
            case "FonctionVCA":
               {
                  List<FonctionVCA> list = await HttpClient.GetFromJsonAsync<List<FonctionVCA>>("VCA/Fonction/");
                  items = list.Cast<object>().ToList();
                  break;
               }
            case "StatusVCA":
               {
                  List<StatusVCA> list = await HttpClient.GetFromJsonAsync<List<StatusVCA>>("VCA/status/");
                  items = list.Cast<object>().ToList();
                  break;
               }
            case "TypeContrat":
               {
                  List<TypeContrat> list = await HttpClient.GetFromJsonAsync<List<TypeContrat>>("type/contrat/");
                  items = list.Cast<object>().ToList();
                  break;
               }
            case "TypeMO":
               {
                  List<TypeMO> list = await HttpClient.GetFromJsonAsync<List<TypeMO>>("type/MO/");
                  items = list.Cast<object>().ToList();
                  break;
               }
         }

         StateHasChanged();
      }

      private async Task CreateModalEdit<T>(string title, T item) where T : class, new()
      {
         var parameters = new Dictionary<string, object>();
         parameters.Add("Item", item);
         parameters.Add("SubmitEvent", EventCallback.Factory.Create<T>(this, Edit));
         await modal.ShowAsync<EditGeneric<T>>(title: title, parameters: parameters);
      }

      private async Task OpenEdit<T>(object id)
      {
         switch (typeof(T))
         {
            case Type t when t == typeof(Fonction):
               {
                  var item = items.Cast<Fonction>().ToList().FirstOrDefault(e => e.Id == (int)id);
                  await CreateModalEdit<Fonction>(localizer["Edition Fonction"], item);
                  break;
               }
            case Type t when t == typeof(FonctionVCA):
               {
                  var item = items.Cast<FonctionVCA>().ToList().FirstOrDefault(e => e.Id == (int)id);
                  await CreateModalEdit<FonctionVCA>(localizer["Edition Fonction VCA"], item);
                  break;
               }
            case Type t when t == typeof(StatusVCA):
               {
                  var item = items.Cast<StatusVCA>().ToList().FirstOrDefault(e => e.Code == (int)id);
                  await CreateModalEdit<StatusVCA>(localizer["Edition Status VCA"], item);
                  break;
               }
            case Type t when t == typeof(TypeContrat):
               {
                  var item = items.Cast<TypeContrat>().ToList().FirstOrDefault(e => e.Code == (int)id);
                  await CreateModalEdit<TypeContrat>(localizer["Edition Type de contrat"], item);
                  break;
               }
            case Type t when t == typeof(TypeMO):
               {
                  var item = items.Cast<TypeMO>().ToList().FirstOrDefault(e => e.Code == id.ToString());
                  await CreateModalEdit<TypeMO>(localizer["Edition Type de main d'oeuvre"], item);
                  break;
               }
         }

      }

      private async Task CreateModalAdd<T>(string title) where T : class, new()
      {
         var item = new T();
         var parameters = new Dictionary<string, object>();
         parameters.Add("Item", item);
         parameters.Add("SubmitEvent", EventCallback.Factory.Create<T>(this, Create));
         await modal.ShowAsync<EditGeneric<T>>(title: title, parameters: parameters);
      }

      private async Task OpenModalAdd()
      {
         switch (type)
         {
            case "Fonction":
               await CreateModalAdd<Fonction>(localizer["Ajout d'une fonction"]);
               break;
            case "FonctionVCA":
               await CreateModalAdd<FonctionVCA>(localizer["Ajout d'une fonction VCA"]);
               break;
            case "StatusVCA":
               await CreateModalAdd<StatusVCA>(localizer["Ajout d'un status VCA"]);
               break;
            case "TypeContrat":
               await CreateModalAdd<TypeContrat>(localizer["Ajout d'un type de contrat"]);
               break;
            case "TypeMO":
               await CreateModalAdd<TypeMO>(localizer["Ajout d'un type de main d'oeuvre"]);
               break;
         }
      }

      public async Task Edit<T>(T entity)
      {
         string url = typeof(T) switch
         {
            Type t when t == typeof(Fonction) => "fonction/",
            Type t when t == typeof(FonctionVCA) => "VCA/Fonction/",
            Type t when t == typeof(StatusVCA) => "VCA/statusVCA/",
            Type t when t == typeof(TypeContrat) => "type/Contrat/",
            Type t when t == typeof(TypeMO) => "type/MO/",
            _ => throw new ArgumentException($"Type {typeof(T)} not supported."),
         };
         await HttpClient.PatchAsJsonAsync<T>(url, entity);
         await modal.HideAsync();
         StateHasChanged();
      }

      public async Task Create<T>(T entity)
      {
         string url = typeof(T) switch
         {
            Type t when t == typeof(Fonction) => "fonction/",
            Type t when t == typeof(FonctionVCA) => "VCA/Fonction/",
            Type t when t == typeof(StatusVCA) => "VCA/statusVCA/",
            Type t when t == typeof(TypeContrat) => "type/Contrat/",
            Type t when t == typeof(TypeMO) => "type/MO/",
            _ => throw new ArgumentException($"Type {typeof(T)} not supported."),
         };
         await HttpClient.PostAsJsonAsync<T>(url, entity);
         await modal.HideAsync();
         await SelectChange(type);
         StateHasChanged();
      }
      private async Task<bool> CreateConfirmation<T>(T entity, string title) where T : class
      {
         var options = new ConfirmDialogOptions { Size = DialogSize.Large };
         var parameters = new Dictionary<string, object>();
         parameters.Add("Item", entity);
         return await dialog.ShowAsync<ShowGeneric<T>>(title, parameters, options);
      }
      public async Task Delete<T>(object id)
      {
         string url;
         bool confirmation = false;
         string title = localizer["Voulez vous vraiment supprimer "];
         switch (typeof(T))
         {
            case var t when t == typeof(Fonction):
               {
                  var item = items.Cast<Fonction>().ToList().FirstOrDefault(e => e.Id == (int)id);
                  confirmation = await CreateConfirmation<Fonction>(item, (title + localizer["cette fonction?"]));
                  url = "Fonction/";
                  break;
               }
            case var t when t == typeof(FonctionVCA):
               {
                  var item = items.Cast<FonctionVCA>().ToList().FirstOrDefault(e => e.Id == (int)id);
                  confirmation = await CreateConfirmation<FonctionVCA>(item, title + localizer["cette fonction VCA?"]);
                  url = "VCA/Fonction/";
                  break;
               }
            case var t when t == typeof(StatusVCA):
               {
                  var item = items.Cast<StatusVCA>().ToList().FirstOrDefault(e => e.Code == (int)id);
                  confirmation = await CreateConfirmation<StatusVCA>(item, title + localizer["ce statut VCA?"]);
                  url = "VCA/statusVCA/";
                  break;
               }
            case var t when t == typeof(TypeContrat):
               {
                  var item = items.Cast<TypeContrat>().ToList().FirstOrDefault(e => e.Code == (int)id);
                  confirmation = await CreateConfirmation<TypeContrat>(item, title + localizer["ce type de contrat?"]);
                  url = "type/Contrat/";
                  break;
               }
            case var t when t == typeof(TypeMO):
               {
                  var item = items.Cast<TypeMO>().ToList().FirstOrDefault(e => e.Code == id.ToString());
                  confirmation = await CreateConfirmation<TypeMO>(item, title + localizer["ce type de main d'oeuvre?"]);
                  url = "type/MO/";
                  break;
               }
            default:
               throw new ArgumentException($"Type {type} not supported.");
         }
         if (confirmation)
         {
            HttpClient.DeleteFromJsonAsync<T>(url + id);
            await Task.Delay(100);
            await SelectChange(type);
            StateHasChanged();
         }

      }
   }
}