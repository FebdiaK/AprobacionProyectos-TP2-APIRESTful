using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces.ServicesInterfaces;
using AprobacionProyectos.Application.Interfaces.PersistenceInterfaces;
using AprobacionProyectos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AprobacionProyectos.Application.Services
{
    public class ApprovalWorkflowService : IApprovalWorkflowService
    {
        private readonly IProjectApprovalStepRepository _stepRepository;
        private readonly IProjectProposalRepository _proposalRepository;
        private readonly IApprovalStatusRepository _statusRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ApprovalWorkflowService(
            IProjectApprovalStepRepository stepRepository,
            IProjectProposalRepository proposalRepository,
            IApprovalStatusRepository statusRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _stepRepository = stepRepository;
            _proposalRepository = proposalRepository;
            _statusRepository = statusRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> ApproveStepAsync(long stepId, int userId, int decisionStatusId, string? observations = null)
        {
            var step = await _stepRepository.GetByIdAsync(stepId);
            if (step == null)
            {
                return false;
            }

            var proposal = await _proposalRepository.GetByIdAsync(step.ProjectProposalId);
            if (proposal == null || proposal.StatusId == 3) // Proyecto ya rechazado (no se puede tomar una decision)
            {
                return false;
            }

            if (decisionStatusId == 1) // El estado pendiente (1) no puede asignarse manualmente
            {
                return false;
            }

            if (step.StatusId != 1 && step.StatusId != 4) // Si ya fue decidido(aprobado o rechazado), no se puede volver a decidir sobre ese paso
            {
                return false;
            }

            var user = await _userRepository.GetByIdAsync(userId);   // Validación: solo el usuario asignado al rol del paso puede decidir:
            if (user == null || step.ApproverRoleId != user.ApproverRole.Id) 
            {
                return false;
            }
            // se requiere que todos los pasos anteriores estén aprobados (StatusId == 2) para avanzar en el flujo de decision
            var allSteps = await _stepRepository.GetStepsByProposalIdAsync(step.ProjectProposalId);
            var currentStepIndex = allSteps.FindIndex(s => s.Id == stepId);
            if (currentStepIndex == -1 || allSteps.Take(currentStepIndex).Any(s => s.StatusId != 2))
                return false;

            var status = await _statusRepository.GetByIdAsync(decisionStatusId);
            if (status == null)
                return false;

            step.StatusId = status.Id; //actualizamos el paso con la decision

            TimeZoneInfo argentinaTimeZone;
            if (OperatingSystem.IsWindows()) { argentinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time"); }
            else { argentinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Argentina/Buenos_Aires"); }

            step.DecisionDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, argentinaTimeZone);

            step.Observations = observations;
            step.ApproverUserId = userId;
           
            if(decisionStatusId == 4 )
            {
                proposal.StatusId = 4; //seteo el proyecto a observado
            }

            if (decisionStatusId == 3)
            {
                proposal.StatusId = 3; // rechazado
            }
            else if (decisionStatusId == 2 && allSteps.All(s => s.Id == stepId || s.StatusId == 2))
            {
                proposal.StatusId = 2; // aprobado si todos los pasos estann aprobados
            }
            
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
