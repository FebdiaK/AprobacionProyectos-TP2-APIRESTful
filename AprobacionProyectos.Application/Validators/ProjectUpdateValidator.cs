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
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El nuevo título es obligatorio.")
                .MinimumLength(5).WithMessage("El Título debe tener al menos 5 caracteres.")
                .MaximumLength(100).WithMessage("El Título no puede superar los 100 caracteres.");


            RuleFor(x => x.description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Ingrese una descripción mínima.")
                .MinimumLength(10).WithMessage("La descripción debe tener al menos 10 caracteres.")
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres."); 


            RuleFor(x => x.duration)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("La duración debe ser mayor a cero.")
                .LessThanOrEqualTo(3650).WithMessage("La duración máxima permitida es de 10 años."); 
        }
    }
}
