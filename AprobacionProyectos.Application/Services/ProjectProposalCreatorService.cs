using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
using AprobacionProyectos.Application.DTOs.Request;
using AprobacionProyectos.Application.Interfaces.ServicesInterfaces;
using AprobacionProyectos.Domain.Entities;
using Azure.Core; 
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using AprobacionProyectos.Application.Interfaces.PersistenceInterfaces;

namespace AprobacionProyectos.Application.Services
{
    public class ProjectProposalCreatorService : IProjectProposalCreatorService
    {
        private readonly IProjectProposalRepository _repository;
        private readonly IApprovalRuleRepository _ruleRepository;
        private readonly IProjectApprovalStepRepository _stepRepository;
        private readonly IApprovalStatusRepository _approvalStatusRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProjectProposalCreatorService(
            IProjectProposalRepository repository,
            IApprovalRuleRepository ruleRepository,
            IProjectApprovalStepRepository stepRepository,
            IApprovalStatusRepository approvalStatusRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _ruleRepository = ruleRepository;
            _stepRepository = stepRepository;
            _approvalStatusRepository = approvalStatusRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> CreateProjectProposalAsync(ProjectProposal proposal)
        {
            TimeZoneInfo argentinaTimeZone;
            if (OperatingSystem.IsWindows()) { argentinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time"); }
            else {argentinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Argentina/Buenos_Aires");}

            proposal.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, argentinaTimeZone);

            var pendingStatus = await _approvalStatusRepository.GetByIdAsync(1);
            proposal.StatusId = pendingStatus.Id;

            await _repository.CreateAsync(proposal);

            var rules = await _ruleRepository.GetAllAsync();

            var applicableRules = rules
                .Where(r =>
                    (r.AreaId == null || r.AreaId == proposal.AreaId) && //si el area es null, sea cual sea el valor del area de la propuesta, entra siempre (si entra en el rango del dinero)
                    (r.TypeId == null || r.TypeId == proposal.TypeId) && //igual que el anterior, pero para el tipo
                    (r.MinAmount <= proposal.EstimatedAmount) &&
                    (r.MaxAmount == 0 || r.MaxAmount >= proposal.EstimatedAmount)) //si maxAmount es 0, entonces es infinito, por ende entra siempre y cuando que el monto supere o sea igual al MinAmount
                .OrderBy(r => r.StepOrder)
                .ToList();

            var selectedRules = applicableRules //se elige la regla mas especifica respecto al area y al tipo
                .GroupBy(r => r.StepOrder)
                .Select(g => g
                    .OrderByDescending(r => (r.AreaId.HasValue ? 1 : 0) + (r.TypeId.HasValue ? 1 : 0)) //priorizo las reglas que tienen area y tipo asignados primero
                    .First())
                .OrderBy(r => r.StepOrder)
                .ToList();

            if (selectedRules.Count == 0) //si no hay reglas aplicables, lanzo una excepcion
            {
                throw new InvalidOperationException("No se encontraron reglas aplicables.");
            }

            foreach (var rule in selectedRules)
            {
                var step = new ProjectApprovalStep  //creo el paso correspondiente a la regla
                {
                    ProjectProposalId = proposal.Id, // y asigno el id de la propuesta al paso
                    ApproverRoleId = rule.ApproverRoleId,
                    Status = pendingStatus,
                    StepOrder = rule.StepOrder,
                    DecisionDate = null,
                    Observations = null
                };

                await _stepRepository.CreateAsync(step); //delego la creacion al repositorio
            }

            await _unitOfWork.SaveChangesAsync(); // guardo los cambios en la db

            return proposal.Id; //retorno el id de la propuesta creada

        }

        public async Task<ProjectProposal?> BuildAsync(CreateProjectProposalRequestDto dto)
        {
            var existing = await _repository.GetProjectProposalByTitle(dto.Title);

            if (existing != null)
            {
                throw new InvalidOperationException("Ya existe un proyecto con ese título.");  // No se permite crear una propuesta con un título ya existente
            }

            var user = await _userRepository.GetByIdAsync(dto.User);
            if (user == null)
            {
                return null;

            }
            return new ProjectProposal
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                EstimatedAmount = dto.Amount,
                EstimatedDuration = dto.Duration,
                AreaId = dto.Area,
                CreatedBy = user,
                TypeId = dto.Type
            };
        }
    }
}
