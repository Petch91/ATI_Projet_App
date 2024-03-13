namespace ATI_Projet_App.Models
{
    public class User
    {
        public int Id { get; set; }
        public int IdExterne { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
