using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.DTOs.Request;
using AprobacionProyectos.Application.Interfaces;
using FluentValidation;

namespace AprobacionProyectos.Application.Validators
{
    public class ProjectDecisionValidator : AbstractValidator<DecisionStepRequestDto> 
    {
        private readonly IUserService _userService;
        private readonly IApprovalStatusService _statusService;

        public ProjectDecisionValidator(IUserService userService, IApprovalStatusService statusService)
        {
            _userService = userService;
            _statusService = statusService;


            RuleFor(x => x.User)
                .GreaterThan(0).WithMessage("El usuario es obligatorio");

            RuleFor(x => x.User)
                .MustAsync(async (id, _) => await _userService.ExistsAsync(id))
                .WithMessage("El usuario no existe, ingrese uno válido")
                .When(x => x.User > 0);


            RuleFor(x => x.Status)
                .GreaterThan(0).WithMessage("El nuevo estado es obligatorio!");

            RuleFor(x => x.Status)
                .MustAsync(async (status, _) => await _statusService.ExistsAsync(status) && status != 1)
                .WithMessage("Se debe pasar un estado válido diferente de pendiente (1)")
                .When(s => s.Status > 0);

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID del paso de aprobación es obligatorio")
                .GreaterThan(0).WithMessage("El ID del paso de aprobación debe ser uno válido");
        }
    }
}
