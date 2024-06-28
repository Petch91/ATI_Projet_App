using ATI_Projet_Tools.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace ATI_Projet_App.Controllers
{
   [Route("[controller]/[action]")]
   public class CultureController(VariablesGlobales variables) : Controller
   {
      private VariablesGlobales _variables = variables;
        public IActionResult Set(string culture, string redirectUri)
      {
         if (culture != null)
         {
            _variables.CurrentCulture = culture; 
            var requestCulture = new RequestCulture(culture, culture);
            var cookieName = CookieRequestCultureProvider.DefaultCookieName;
            var cookieValue = CookieRequestCultureProvider.MakeCookieValue(requestCulture);

            HttpContext.Response.Cookies.Append(cookieName, cookieValue);
         }
         return LocalRedirect(redirectUri);
      }
   }
}
