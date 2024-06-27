using ATI_Projet_App.Controllers;
using ATI_Projet_App.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ATI_Projet_App.Tools
{
   public class SessionManager(ProtectedLocalStorage protectedLocalStorage, ProtectedSessionStorage protectedSessionStorage, HttpClient httpClient, NavigationManager navigationManager)
   {
      private readonly ProtectedLocalStorage _protectedLocalStorage = protectedLocalStorage;
      private readonly ProtectedSessionStorage _protectedSessionStorage = protectedSessionStorage;
      private readonly HttpClient _client = httpClient;
      private readonly NavigationManager _navigationManager = navigationManager;

      public User? ConnectedUser
      {
         //get
         //{
         //    return Get<User>("ConnectedUser").Result;                   
         //}
         set
         {
            Set<User>("ConnectedUser", value);
         }
      }

      public string Token
      {
         //get
         //{
         //    return Get<string>("Token").Result;
         //}
         set
         {
            Set<string>("Token", value);
         }
      }

      //public async Task<T> Get<T>(string key)
      //{
      //    var result = await _protectedLocalStorage.GetAsync<T>(key);
      //    return result.Value;
      //}

      private async Task Set<T>(string key, T value)
      {
         await _protectedLocalStorage.SetAsync(key, value);
      }

      public async Task<User> GetUser()
      {
         var result = await _protectedLocalStorage.GetAsync<User>("ConnectedUser");
         return result.Value;
      }

      public async Task<string> GetToken()
      {
         var result = await _protectedLocalStorage.GetAsync<string>("Token");
         return result.Value;
      }

      public async Task Logout()
      {
         await _protectedLocalStorage.DeleteAsync("ConnectedUser");
         await _protectedLocalStorage.DeleteAsync("Token");

      }
      public async Task<bool> Login(int Id, string HashToken)
      {

         //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Value);
         using (HttpResponseMessage response = await _client.PostAsJsonAsync<LoginExt>($"http://192.168.123.69:7000/api/user/token/", new LoginExt { Id = Id, HashToken = HashToken }))
         {
            if (response.IsSuccessStatusCode)
            {
               return true;
            }
            else
            {
               return false;
            }

         }
      }
      public async Task<T> GetSessionStorage<T>(string key)
      {
         var result = await _protectedSessionStorage.GetAsync<T>(key);
         return result.Value ?? default!;
      }
      public async void SetSessionStorage<T>(string key, T obj)
      {
         if(obj != null)
         await _protectedSessionStorage.SetAsync(key, obj);
      }
   }
}
   
