using ATI_Projet_App.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models.CallRecords;

namespace ATI_Projet_App.Controllers
{
   [Route("[controller]/[action]")]
   [ApiController]
   public class LoginController(SessionManager sessionManager) : ControllerBase
   {
      private readonly SessionManager _sessionManager = sessionManager;

      [HttpPost]
      public IActionResult Login([FromBody] LoginExt login)
      {
         if (_sessionManager.Login(login.Id, login.HashToken).Result) return Ok();
         else return BadRequest();
      }
   }
   public class LoginExt
   {
      public int Id { get; set; }
      public string HashToken { get; set; }
   }

}
