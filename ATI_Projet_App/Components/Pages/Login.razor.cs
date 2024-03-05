using ATI_Projet_App.Models;
using ATI_Projet_App.Models.Forms;
using ATI_Projet_App.Tools;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace ATI_Projet_App.Components.Pages
{
    public partial class Login
    {
        [Inject]
        private HttpClient http {  get; set; }

        [Inject]
        private SessionManager session { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }
        public LoginForm Form { get; set; } = new LoginForm();

        public void Login_()
        {
            string token = "";
            string jsonToSend = JsonConvert.SerializeObject(Form);
            HttpContent content = new StringContent(jsonToSend, Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = http.PostAsync("User/login", content).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    token = response.Content.ReadAsStringAsync().Result;
                    session.Token = token;
                    JwtSecurityToken jwt = new JwtSecurityToken(token);
                    int id = int.Parse(jwt.Claims.First(x => x.Type == ClaimTypes.Sid).Value);
                    using (HttpResponseMessage responseGet = http.GetAsync("user/" + id).Result)
                    {
                        if (responseGet.IsSuccessStatusCode)
                        {
                            string json = responseGet.Content.ReadAsStringAsync().Result;
                            session.ConnectedUser = JsonConvert.DeserializeObject<User>(json);
                            StateHasChanged();
                            navigationManager.NavigateTo("/");
                        }
                        else
                        {
                            throw new Exception(responseGet.StatusCode.ToString());
                        }
                    }
                }
                else { throw new Exception("Email ou mot de passe invalide"); }
            }

        }
    }
}