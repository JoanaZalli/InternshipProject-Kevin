using Application.UseCases.Permissions.Commands;
using Application.UseCases.Permissions.Queries;
using Application.UseCases.ViewPermissions.Queries;
using AutoMapper;
using Domain.Seeds;
using InternshipProject.Objects.Requests.PermissionRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InternshipProject.Controllers {

    [Route("api")]
    [ApiController]
    public class PermissionController : ControllerBase {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PermissionController(IMediator mediator, IMapper mapper) {
            _mediator = mediator;
            _mapper = mapper;
        }

        [SwaggerOperation(Summary = "Get all permissions")]
        [Authorize(Policy = DefinedPermissions.IsSuperAdmin.Name)]
        [HttpGet("permissions")]
        public async Task<IActionResult> GetPermissions([FromQuery] string? filter,
                                                        [FromQuery] string? sortColumn,
                                                        [FromQuery] string? sortOrder,
                                                        [FromQuery] int pageSize,
                                                        [FromQuery] int page) {
            var result = await _mediator.Send(new GetAllPermissionsQuery {
                Filter = filter,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
                PageSize = pageSize,
                Page = page
            });
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get permissions by given role id")]
        [Authorize(Policy = DefinedPermissions.IsSuperAdmin.Name)]
        [HttpGet("roles/{id}/permissions")]
        public async Task<IActionResult> GetRolePermissions([FromRoute] Guid id,
                                                            [FromQuery] string? filter,
                                                            [FromQuery] string? sortColumn,
                                                            [FromQuery] string? sortOrder,
                                                            [FromQuery] int pageSize,
                                                            [FromQuery] int page) {
            var result = await _mediator.Send(new RolePermissionsQuery {
                Id = id,
                Filter = filter,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
                PageSize = pageSize,
                Page = page
            });
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Create new permission")]
        [Authorize(Policy = DefinedPermissions.IsSuperAdmin.Name)]
        [HttpPost("permissions")]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionRequest permissionRequest) {
            var command = _mapper.Map<CreatePermissionCommand>(permissionRequest);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Delete permission")]
        [Authorize(Policy = DefinedPermissions.IsSuperAdmin.Name)]
        [HttpDelete("permissions/{id}")]
        public async Task<IActionResult> DeletePermission([FromRoute] Guid id) {
            var command = new DeletePermissionCommand { PermissionId = id };
            await _mediator.Send(command);
            return Ok();
        }
    }
}
