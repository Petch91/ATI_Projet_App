using Blazorise;
using FluentValidation;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATI_Projet_Models.Validators
{
   public class AdresseValidator : AbstractValidator<Adresse>
   {
      public AdresseValidator()
      {
         RuleFor(a => a.adresse)
            .NotEmpty().WithMessage("La rue et le numéro sont requis");

         if (CultureInfo.CurrentCulture.Equals(new CultureInfo("fr-BE")))
         {
            RuleFor(a => a.CodePostal)
               .NotEmpty().WithMessage("Code Postal Requis")
               .Must(cp => cp >= 1000 && cp <= 9999).WithMessage("Ceci n'est pas un code postal valide");
         }
         else
         {
            RuleFor(a => a.CodePostal)
               .NotEmpty().WithMessage("Zip Postal Code Required")
               .Must(cp => cp >= 5000 && cp <= 99999).WithMessage("This is not a valid postcode");
         }

         RuleFor(a => a.Localite)
            .NotEmpty().WithMessage("Une localité est requise");

         RuleFor(a => a.Pays)
            .NotEmpty().WithMessage("Un Pays est requis");
      }
   }
}
