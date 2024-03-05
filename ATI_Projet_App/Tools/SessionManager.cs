using ATI_Projet_App.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;

namespace ATI_Projet_App.Tools
{
    public class SessionManager(ProtectedLocalStorage protectedLocalStorage )
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage = protectedLocalStorage;


        public User? ConnectedUser
        {
            //get
            //{
            //    return Get<User>("ConnectedUser").Result;                   
            //}
            set
            {
                Set<User> ("ConnectedUser", value);
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

        private async Task Set<T>(string key , T value)
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
    }
}
