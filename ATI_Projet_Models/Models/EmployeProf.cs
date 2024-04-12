using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models
{
    public class EmployeProf
    {
        public int Id { get; set; }
        [DisplayName("Coût")]
        public int? Cout { get; set; }
        public string? SafetyBook { get; set; }
        public DateTime DateSafetyBook { get; set; }
        public string? Badge { get; set; }
    }
}
