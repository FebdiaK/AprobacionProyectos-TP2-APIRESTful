using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Infrastructure.Data;
using AprobacionProyectos.Infrastructure.Repositories.Implementations;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;

namespace AprobacionProyectos.Application.Services
{
    public class ApprovalWorkflowService : IApprovalWorkflowService
    {
        private readonly IProjectApprovalStepRepository _stepRepository;
        private readonly IProjectProposalRepository _proposalRepository;
        private readonly IApprovalStatusRepository _statusRepository;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;
        public ApprovalWorkflowService(
            IProjectApprovalStepRepository stepRepository,
            IProjectProposalRepository proposalRepository,
            IApprovalStatusRepository statusRepository,
            IUserRepository userRepository,
            AppDbContext context)
        {
            _stepRepository = stepRepository;
            _proposalRepository = proposalRepository;
            _statusRepository = statusRepository;
            _userRepository = userRepository;
            _context = context;
        }
        public async Task<bool> ApproveStepAsync(long stepId, int userId, int decisionStatusId, string? observations = null)
        {
            var step = await _stepRepository.GetByIdAsync(stepId);
            if (step == null)
            {
                return false;
            }
            if (step.StatusId != 1 && step.StatusId != 4) // Si ya fue decidido y NO está en estado observado (4), no se puede volver a decidir
            {
                return false;
            }

            var user = await _userRepository.GetByIdAsync(userId);   // Validación: solo el usuario asignado puede decidir
            if (user == null)
            {
                return false;
            }

            if (step.ApproverRoleId != user.ApproverRole.Id) //// Validar que el usuario tenga el rol requerido para ese paso
            {
                return false;
            }

            var allSteps = await _stepRepository.GetStepsByProposalIdAsync(step.ProjectProposalId);
            var currentStepIndex = allSteps.FindIndex(s => s.Id == stepId);

            if (currentStepIndex == -1)
                return false; // El paso no existe

            // No se puede decidir si hay pasos anteriores que no estén aprobados
            var previousSteps = allSteps.Take(currentStepIndex);
            bool allPreviousApproved = previousSteps.All(s => s.StatusId == 2);
            if (!allPreviousApproved)
                return false;

            var status = await _statusRepository.GetByIdAsync(decisionStatusId); 
            step.StatusId = status.Id; //actualizamos el paso con la decision
            step.DecisionDate = DateTime.UtcNow;
            step.Observations = observations;
            step.ApproverUserId = userId;


            var proposal = await _proposalRepository.GetByIdAsync(step.ProjectProposalId);
            if (proposal != null)
            {
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
            }
            // Si el paso es observado, no se cambia el estado de la propuesta

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
