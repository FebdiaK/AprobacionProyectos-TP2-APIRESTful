using AprobacionProyectos.Application.DTOs.Request;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AprobacionProyectos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectProposalCreatorService _creatorService;
        private readonly IProjectProposalQueryService _queryService;

        public ProjectController(
            IProjectProposalCreatorService creatorService,
            IProjectProposalQueryService repository)
        {
            _creatorService = creatorService;
            _queryService = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectProposalDto dto)
        {
            // Validación: no repetir títulos
            var existing = await _queryService.GetProjectProposalByTitle(dto.Title);
            if (existing != null)
            {
                return Conflict(new { message = "Ya existe un proyecto con ese título." });
            }

            var proposal = new ProjectProposal
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                EstimatedAmount = dto.EstimatedAmount,
                AreaId = dto.AreaId,
                TypeId = dto.TypeId,
                CreatedById = dto.CreatedById,
            };

            var id = await _creatorService.CreateProjectProposalAsync(proposal);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var project = await _queryService.GetProjectProposalFullWithId(id);
            if (project == null)
                return NotFound();

            return Ok(project);
        }
    }
}
