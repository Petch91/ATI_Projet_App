using ATI_Projet_App.Models;
using ATI_Projet_App.Tools;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace ATI_Projet_App.Components.Pages
{
    public partial class Authe
    {
        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public string HashToken { get; set; }
        [Inject]
        private HttpClient _client { get; set; }
        [Inject]
        private ProtectedLocalStorage storage { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }
        [Inject]
        private SessionManager sessionManager { get; set; }

        private User? connectedUser;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //return base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                ProtectedBrowserStorageResult<string> result = new ProtectedBrowserStorageResult<string>();
                try
                {
                    result = await storage.GetAsync<string>("Token");
                }
                catch (CryptographicException e)
                {
                    await storage.DeleteAsync("Token");
                    await storage.DeleteAsync("ConnectedUser");
                    result = await storage.GetAsync<string>("Token");
                }
                string token = result.Value;
                if (token != null)
                {
                    await sessionManager.Logout();
                }
                try
                {
                    //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Value);
                    using (HttpResponseMessage response = _client.GetAsync($"http://localhost:7000/api/user/token/{Id}/{HashToken}").Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            token = response.Content.ReadAsStringAsync().Result;
                            if (token != null)
                            {
                                JwtSecurityToken jwt = new JwtSecurityToken(token);
                                if (jwt.ValidTo >= DateTime.UtcNow)
                                {
                                    ;
                                    int id = int.Parse(jwt.Claims.First(x => x.Type == ClaimTypes.Sid).Value);
                                    using (HttpResponseMessage res = _client.GetAsync("http://localhost:7000/api/user/byid/" + id).Result)
                                    {
                                        if (res.IsSuccessStatusCode)
                                        {
                                            string json = res.Content.ReadAsStringAsync().Result;
                                            sessionManager.ConnectedUser = JsonConvert.DeserializeObject<User>(json);
                                            sessionManager.Token = token;
                                            StateHasChanged();
                                            navigationManager.NavigateTo("/", true);
                                        }
                                        else
                                        {
                                            throw new Exception(res.StatusCode.ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    await sessionManager.Logout();
                                    navigationManager.NavigateTo("http://192.168.123.238:7100/logout", true);
                                }

                            }

                        }
                        else
                        {
                            return;
                        }

                    }
                }
                catch (Exception ex)
                {
                    await sessionManager.Logout();
                    navigationManager.NavigateTo("http://192.168.123.238:7100/logout", true);
                }

            }
        }

    }
}
