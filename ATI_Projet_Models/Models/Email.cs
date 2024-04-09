using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models
{
    public class Email
    {
        public int Id { get; set; }
        [EmailAddress]
        public string email { get; set; }
        public string? Description { get; set; }
        public int? PersonneId { get; set; }
        public int? SocieteId { get; set; }

        public Email()
        {
            
        }
        public Email( Email e)
        {
            Id = e.Id;
            Description = e.Description;
            PersonneId = e.PersonneId;
            SocieteId = e.SocieteId;
            email = e.email;
        }
    }
}
