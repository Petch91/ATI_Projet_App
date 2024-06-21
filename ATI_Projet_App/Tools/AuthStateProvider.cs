using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ATI_Projet_App.Tools
{
   public class AuthStateProvider(ProtectedLocalStorage storage, NavigationManager navigationManager) : AuthenticationStateProvider
   {
      private readonly ProtectedLocalStorage _storage = storage;
      private readonly NavigationManager _navigationManager = navigationManager;
      private bool IsOk;

      public override async Task<AuthenticationState> GetAuthenticationStateAsync()
      {
         List<Claim> claims = new List<Claim>();
         ProtectedBrowserStorageResult<string> result = new ProtectedBrowserStorageResult<string>();

         try
         {
            result = await _storage.GetAsync<string>("Token");
         }
         catch (CryptographicException e)
         {
            await _storage.DeleteAsync("Token");
            await _storage.DeleteAsync("ConnectedUser");
            _navigationManager.Refresh(true);
            result = await _storage.GetAsync<string>("Token");
         }

         string token = result.Value;

         if (string.IsNullOrWhiteSpace(token))
         {
            ClaimsIdentity anonymousUser = new ClaimsIdentity();
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymousUser)));
         }

         JwtSecurityToken jwt = new JwtSecurityToken(token);
         if (jwt.ValidTo < DateTime.UtcNow)
         {
            await _storage.DeleteAsync("Token");
            await _storage.DeleteAsync("ConnectedUser");
            ClaimsIdentity anonymousUser = new ClaimsIdentity();
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymousUser)));
         }
         foreach (Claim claim in jwt.Claims)
         {
            claims.Add(claim);
         }

         ClaimsIdentity currentUser = new ClaimsIdentity(claims, "TestAuthSystem");


         return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(currentUser))).Result;
      }
      public void NotifyUserChanged()
      {
         NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
      }
   }
}
