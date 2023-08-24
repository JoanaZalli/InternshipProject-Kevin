using Application.UseCases.BlockedAccounts.Queries;
using Application.UseCases.Permissions.Commands;
using Application.UseCases.Roles.Commands;
using Application.UseCases.UnblockAccount.Command;
using AutoMapper;
using Domain.Seeds;
using InternshipProject.Objects.Requests.ManagementRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InternshipProject.Controllers {

    [Route("api/management")]
    [ApiController]
    public class ManagementController : ControllerBase {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ManagementController(IMediator mediator, IMapper mapper) {
            _mediator = mediator;
            _mapper = mapper;
        }

        [SwaggerOperation(Summary = "Assign a role to a user")]
        [Authorize(Policy = DefinedPermissions.IsSuperAdmin.Name)]
        [HttpPost("users/{id}/roles")]
        public async Task<IActionResult> AssignRole([FromRoute] Guid id, [FromBody] AssignationRequest assignationRequest) {
            var command = _mapper.Map<RoleAssignationCommand>(assignationRequest);
            command.UserId = id;
            await _mediator.Send(command);
            return Ok();
        }

        [SwaggerOperation(Summary = "Assing a permission to a role")]
        [Authorize(Policy = DefinedPermissions.IsSuperAdmin.Name)]
        [HttpPost("roles/{id}/permissions")]
        public async Task<IActionResult> AssignPermission([FromRoute] Guid id, [FromBody] AssignationRequest assignationRequest) {
            var command = _mapper.Map<PermissionAssignationCommand>(assignationRequest);
            command.RoleId = id;
            await _mediator.Send(command);
            return Ok();
        }

        [SwaggerOperation(Summary = "Get all blocked account")]
        [Authorize(Policy = DefinedPermissions.IsSuperAdmin.Name)]
        [HttpGet("blocked-accounts")]
        public async Task<IActionResult> GetBlockedAccounts([FromQuery] string? filter,
                                                            [FromQuery] string? sortColumn,
                                                            [FromQuery] string? sortOrder,
                                                            [FromQuery] int pageSize,
                                                            [FromQuery] int page) {
            var response = await _mediator.Send(new BlockedAccountsQuery {
                Filter = filter,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
                PageSize = pageSize,
                Page = page
            });
            return Ok(response);
        }

        [SwaggerOperation(Summary = "Unblock a blocked account")]
        [Authorize(Policy = DefinedPermissions.IsSuperAdmin.Name)]
        [HttpPatch("unblock-account/{id}")]
        public async Task<IActionResult> UnblockAccount([FromRoute] Guid id) {
            await _mediator.Send(new UnblockAccountCommand {
                UserId = id
            });
            return Ok();
        }
    }
}
