using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.DTOs.Request;
using AprobacionProyectos.Application.Interfaces.ServicesInterfaces;
using FluentValidation;

namespace AprobacionProyectos.Application.Validators
{
    public class ProjectQueryValidator : AbstractValidator<ProjectQueryRequestDto>  

    {
        private readonly IUserService _userService;
        private readonly IApprovalStatusService _approvalStatusService;
        public ProjectQueryValidator( IUserService userService, IApprovalStatusService approvalStatusService)
        {
            _userService = userService;
            _approvalStatusService = approvalStatusService;

            RuleFor(x => x.status) 
                .Must(x => !x.HasValue || x > 0)
                .WithMessage("El status debe ser mayor a cero o null.");

            RuleFor(x => x.status)
                .MustAsync(async (id, _) => await _approvalStatusService.ExistsAsync(id!.Value))
                .WithMessage("El parámetro 'status' debe tener un ID existente.")
                .When(x => x.status.HasValue);


            RuleFor(x => x.applicant)
                .Must(x => !x.HasValue || x > 0)
                .WithMessage("El parámetro 'applicant' debe ser mayor a cero o null.");

            RuleFor(x => x.applicant)
                .MustAsync(async (id, _) => await _userService.ExistsAsync(id!.Value))
                .WithMessage("El parámetro 'applicant' debe ser un ID de usuario existente.")
                .When(x => x.applicant.HasValue);


            RuleFor(x => x.approvalUser)
                .Must(x => !x.HasValue || x > 0)
                .WithMessage("El parámetro 'approvalUser' debe ser mayor a cero o null.");

            RuleFor(x => x.approvalUser)
                .MustAsync(async (id, _) => await _userService.ExistsAsync(id!.Value))
                .WithMessage("El parámetro 'approvalUser' debe ser un ID de usuario existente.")
                .When(x => x.approvalUser.HasValue);


            RuleFor(x => x.title)
                .MaximumLength(100)
                .WithMessage("El parámetro 'title' no puede exceder los 100 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.title)); 
        }
    }
}
