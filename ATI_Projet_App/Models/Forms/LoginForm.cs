using System.ComponentModel.DataAnnotations;

namespace ATI_Projet_App.Models.Forms
{
    public class LoginForm
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
