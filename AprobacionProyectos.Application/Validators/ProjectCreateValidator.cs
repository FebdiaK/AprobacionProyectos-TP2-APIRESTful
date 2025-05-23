using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.DTOs.Request;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;
using FluentValidation;

namespace AprobacionProyectos.Application.Validators
{
    public class ProjectCreateValidator : AbstractValidator<CreateProjectProposalRequestDto>
    {
        private readonly IAreaService _areaService;
        private readonly IUserService _userService;
        private readonly IProjectTypeService _typeService;

        public ProjectCreateValidator(
        IAreaService areaService,
        IUserService userService,
        IProjectTypeService typeService)
        {
            _areaService = areaService;
            _userService = userService;
            _typeService = typeService;
        
       
            RuleFor(x => x.Title)
                    .NotEmpty().WithMessage("El título es obligatorio");

            RuleFor(x => x.Amount)
                    .GreaterThan(0).WithMessage("El monto debe ser mayor a 0");

            RuleFor(x => x.Duration)
                    .GreaterThan(0).WithMessage("La duración debe ser mayor a 0");

            RuleFor(x => x.Area)
                .GreaterThan(0).WithMessage("El area debe tener un ID válido")
                    .MustAsync(async (id, _) => await _areaService.ExistsAsync(id))
                    .WithMessage("El área especificada no existe");

            RuleFor(x => x.Type)
                    .GreaterThan(0).WithMessage("El tipo de proyecto debe tener un ID válido")
                    .MustAsync(async (id, _) => await _typeService.ExistsAsync(id))
                    .WithMessage("El tipo de proyecto especificado no existe");

            RuleFor(x => x.User)
                    .GreaterThan(0).WithMessage("El usuario debe tener un ID válido")
                    .MustAsync(async (id, _) => await _userService.ExistsAsync(id))
                    .WithMessage("El usuario especificado no existe");
        }
    }
}


