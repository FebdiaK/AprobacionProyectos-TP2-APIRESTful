using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Application.DTOs.Request;
using FluentValidation;

namespace AprobacionProyectos.Application.Validators
{
    public class ProjectCreateValidator2 : AbstractValidator<CreateProjectProposalRequestDto> 
    {
        private readonly IAreaService _areaService;
        private readonly IUserService _userService;
        private readonly IProjectTypeService _typeService;

        public ProjectCreateValidator2(
            IAreaService areaService,
            IUserService userService,
            IProjectTypeService typeService)
        {
            _areaService = areaService;
            _userService = userService;
            _typeService = typeService;


            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("El Título es obligatorio.")
                .MinimumLength(5).WithMessage("El Título debe tener al menos 5 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Ingrese una descripción mínima.")
                .MinimumLength(10).WithMessage("La descripción debe tener al menos 10 caracteres.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a cero.");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("La duración debe ser mayor a cero.");

            RuleFor(x => x.Area)
                .GreaterThan(0).WithMessage("El área es obligatoria.");

            RuleFor(x => x.Area)
                .MustAsync(async (areaId, _) => await _areaService.ExistsAsync(areaId))
                .WithMessage("Se debe pasar un área válida.")
                .When(s => s.Area > 0);

            RuleFor(x => x.User)
                .GreaterThan(0).WithMessage("El usuario es obligatorio");

            RuleFor(x => x.User)
                .MustAsync(async (id, _) => await _userService.ExistsAsync(id))
                .WithMessage("El usuario no existe, ingrese uno válido")
                .When(x => x.User > 0);

            RuleFor(x => x.Type)
                .GreaterThan(0).WithMessage("El tipo de proyecto es obligatorio.");

            RuleFor(x => x.Type)
                .MustAsync(async (id, _) => await _typeService.ExistsAsync(id))
                .WithMessage("El tipo de proyecto no existe, ingrese uno válido")
                .When(x => x.Type > 0);

        }
    }
}
