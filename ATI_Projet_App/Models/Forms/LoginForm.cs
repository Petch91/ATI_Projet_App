using System.ComponentModel.DataAnnotations;

namespace ATI_Projet_App.Models.Forms
{
    public class LoginForm
    {
        [Required]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
