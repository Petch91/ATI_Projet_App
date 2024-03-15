using System.ComponentModel;

namespace ATI_Projet_Models
{
    public class Personnel
    {
        public int Id { get; set; }
        public string? Titre { get; set; }
        [DisplayName("Nom")]
        public string Name { get; set; }
        public string Email { get; set; }
        public string? GSM { get; set; }
        public string? Fonction { get; set; }
        public string? Vehicule { get; set; }
    }
}
