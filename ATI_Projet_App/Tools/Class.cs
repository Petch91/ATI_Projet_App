using Microsoft.JSInterop;

namespace ATI_Projet_App.Tools
{
   public class JSTools(IJSRuntime jSRuntime)
   {
      private IJSRuntime _jSRuntime = jSRuntime;

      public async Task<string> IsoToEmoji(string code)
      {
         string c = await _jSRuntime.InvokeAsync<string>("isoToEmoji",code);
         return c;
      }
   }

}
