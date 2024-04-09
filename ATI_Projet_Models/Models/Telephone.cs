using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models
{
    public class Telephone
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string? Description { get; set; }
        public int? PersonneId { get; set; }
        public int? SocieteId { get; set; }

        public Telephone()
        {
            
        }

        public Telephone( Telephone telephone)
        {
            Id = telephone.Id;
            Numero = telephone.Numero;
            Description = telephone.Description;
            PersonneId = telephone.PersonneId;
            SocieteId = telephone.SocieteId;
        }
    }
}
