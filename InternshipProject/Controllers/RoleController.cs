using Application.UseCases.Roles.Commands;
using Application.UseCases.Roles.Queries;
using Application.UseCases.ViewPermissions.Queries;
using AutoMapper;
using Domain.Seeds;
using InternshipProject.Objects.Requests.RoleRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InternshipProject.Controllers {

    [ApiController]
    [Route("api")]
    public class RoleController : ControllerBase {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RoleController(IMediator mediator, IMapper mapper) {
            _mediator = mediator;
            _mapper = mapper;
        }

        [SwaggerOperation(Summary = "Get all roles")]
        [Authorize(Policy = DefinedPermissions.IsSuperAdmin.Name)]
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles([FromQuery] string? filter,
                                                  [FromQuery] string? sortColumn,
                                                  [FromQuery] string? sortOrder,
                                                  [FromQuery] int pageSize,
                                                  [FromQuery] int page) {
            var result = await _mediator.Send(new GetRoleQuery {
                Filter = filter,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
                PageSize = pageSize,
                Page = page
            });
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get roles by given user id")]
        [Authorize(Policy = DefinedPermissions.IsSuperAdmin.Name)]
        [HttpGet("users/{id}/roles")]
        public async Task<IActionResult> GetUserRoles([FromRoute] Guid id,
                                                      [FromQuery] string? filter,
                                                      [FromQuery] string? sortColumn,
                                                      [FromQuery] string? sortOrder,
                                                      [FromQuery] int pageSize,
                                                      [FromQuery] int page) {
            var result = await _mediator.Send(new UserRoleQuery {
                UserId = id,
                Filter = filter,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
                PageSize = pageSize,
                Page = page
            });
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Create new role")]
        [Authorize(Policy = DefinedPermissions.IsSuperAdmin.Name)]
        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole([FromBody] RoleRequest roleRequest) {
            var command = _mapper.Map<CreateRoleCommand>(roleRequest);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Delete role")]
        [Authorize(Policy = DefinedPermissions.IsSuperAdmin.Name)]
        [HttpDelete("roles/{id}")]
        public async Task<IActionResult> DeleteRole([FromRoute] Guid id) {
            var command = new DeleteRoleCommand { RoleId = id };
            await _mediator.Send(command);
            return Ok();
        }
    }
}
