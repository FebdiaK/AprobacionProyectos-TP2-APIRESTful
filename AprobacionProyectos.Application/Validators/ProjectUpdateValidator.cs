using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.DTOs.Request;
using FluentValidation;

namespace AprobacionProyectos.Application.Validators
{
    public class ProjectUpdateValidator : AbstractValidator<UpdateProjectProposalRequestDto>
    {
        public ProjectUpdateValidator()
        {
            RuleFor(x => x.duration)
                .GreaterThan(0).WithMessage("La duración debe ser un número mayor de cero.");

            RuleFor(x => x.title)
                .NotEmpty().When(x => !string.IsNullOrWhiteSpace(x.title))
                .MinimumLength(5).WithMessage("El título debe tener al menos 5 caracteres.")
                .MaximumLength(200).WithMessage("El título no debe exceder los 200 caracteres.");

            RuleFor(x => x.description)
                .NotEmpty().When(x => !string.IsNullOrWhiteSpace(x.description))
                .MinimumLength(10).WithMessage("La descripción debe tener al menos 10 caracteres.")
                .MaximumLength(1000).WithMessage("La descripción no debe exceder los 1000 caracteres.");
        }
    }
}
