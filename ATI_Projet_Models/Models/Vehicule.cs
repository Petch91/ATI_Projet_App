using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models.Models
{
    public class Vehicule
    {
        public int Id { get; set; }
        public string Designation { get; set; }
        [DisplayName("Kilomètres Total")]
        public int KmTotal { get; set; }
        public string Motorisation { get; set; }
    }
}
