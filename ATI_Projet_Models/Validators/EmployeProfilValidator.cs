using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models.Validators
{
    public class EmployeProfilValidator : AbstractValidator<EmployeProfil>
    {
        public EmployeProfilValidator()
        {
            RuleFor(e => e.Initiales)
                .Length(3).WithMessage("Les Initiales doivent juste avoir 3 caractères")
                .NotEmpty().WithMessage("Les initiales sont requis");

            RuleFor(e => e.Nom)
                .NotEmpty().WithMessage("Le nom est requis");

            RuleFor(e => e.Prenom)
                .NotEmpty().WithMessage(@"Le prénom est requis");
            RuleFor(e => e.DateEntree)
                .Must(d => d <= DateTime.Now).WithMessage(@"La date ne peut pas dépasser la date d'aujourd'hui");

            RuleFor(e => e.DateSortie)
                .Must(d => d <= DateTime.Now).WithMessage(@"La date ne peut pas dépasser la date d'aujourd'hui")
                .Must((e,d) => e.Actif || d >= e.DateEntree).WithMessage(@"La date de sortie doit être postérieure ou égale à la date d'entrée");

        }
    }
}
