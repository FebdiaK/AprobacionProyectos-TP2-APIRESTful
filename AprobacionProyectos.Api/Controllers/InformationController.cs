using AprobacionProyectos.Application.DTOs.EntitiesDTOs;
using AprobacionProyectos.Application.Interfaces.ServicesInterfaces;
using AprobacionProyectos.Application.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AprobacionProyectos.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class InformationController : ControllerBase
    {
        private readonly IAreaService _areaService;
        private readonly IProjectTypeService _projectTypeService;
        private readonly IApprovalStatusService _approvalStatusService;
        private readonly IApproverRoleService _roleService;
        private readonly IUserService _userService;


        public InformationController
            (IAreaService areaService,
            IProjectTypeService projectTypeService,
            IApprovalStatusService approvalStatusService,
            IApproverRoleService roleService,
            IUserService userService)
        {
            _areaService = areaService;
            _projectTypeService = projectTypeService;
            _approvalStatusService = approvalStatusService;
            _roleService = roleService;
            _userService = userService;
        }


        [HttpGet("Area")]
        public async Task<ActionResult<List<AreaDto>>> GetAreas()
        {
            var areas = await _areaService.GetAllAreasAsync();

            var result = areas.Select(a => a.ToDto()).ToList();

            return Ok(result);
        }

        [HttpGet("ProjectType")]
        public async Task<ActionResult<List<TypeDto>>> GetProjectTypes()
        {
            var types = await _projectTypeService.GetAllProjectTypesAsync();

            var result = types.Select(t => t.ToDto()).ToList();

            return Ok(result);
        }

        [HttpGet("ApprovalStatus")]
        public async Task<ActionResult<List<StatusDto>>> GetStatuses()
        {
            var statuses = await _approvalStatusService.GetAllApprovalStatusesAsync();

            var result = statuses.Select(s => s.ToDto()).ToList();

            return Ok(result);
        }

        [HttpGet("Role")]
        public async Task<ActionResult<List<ApproverRoleDto>>> GetRoles()
        {
            var roles =  await _roleService.GetAllApproverRolesAsync();

            var result = roles.Select(r => r.ToDto()).ToList();

            return Ok(result);
        }

        [HttpGet("User")]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            var result = users.Select(u => u.ToDto()).ToList();

            return Ok(result);

        }
    }
}
