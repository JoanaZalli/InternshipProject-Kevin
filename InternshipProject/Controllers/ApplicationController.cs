using Application.UseCases.ApplicationJourney.Commands;
using Application.UseCases.ApplicationJourney.Queries;
using AutoMapper;
using Domain.Seeds;
using InternshipProject.Objects.Requests.ApplicationJourneyRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InternshipProject.Controllers {

    [ApiController]
    [Route("api")]
    public class ApplicationController : ControllerBase {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ApplicationController(IMapper mapper, IMediator mediator) {
            _mapper = mapper;
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Create new application")]
        [Authorize(Policy = $"{DefinedPermissions.CanAddApplication.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpPost("borrowers/{id}/applications")]
        public async Task<IActionResult> CreateApplication([FromRoute] Guid id, [FromBody] CreateApplicationRequest request) {
            var command = _mapper.Map<CreateApplicationCommand>(request);
            command.BorrowerId = id;

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get application info by id")]
        [Authorize(Policy = $"{DefinedPermissions.CanReadApplications.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpGet("applications/{id}")]
        public async Task<IActionResult> GetApplication([FromRoute] Guid id) {
            var result = await _mediator.Send(new GetApplicationQuery {
                ApplicationId = id
            });
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get all applications")]
        [Authorize(Policy = $"{DefinedPermissions.CanReadApplications.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpGet("applications")]
        public async Task<IActionResult> GetAllApplications([FromQuery] string? filter,
                                                            [FromQuery] string? sortColumn,
                                                            [FromQuery] string? sortOrder,
                                                            [FromQuery] int pageSize,
                                                            [FromQuery] int page) {
            var result = await _mediator.Send(new GetAllApplicationsQuery {
                Filter = filter,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
                PageSize = pageSize,
                Page = page
            });
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Edit application info (Loan Officer)")]
        [Authorize(Policy = $"{DefinedPermissions.CanUpdateApplication.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpPut("applications/{id}")]
        public async Task<IActionResult> UpdateApplication([FromRoute] Guid id, [FromBody] UpdateApplicationRequest request) {
            var command = _mapper.Map<UpdateApplicationCommand>(request);
            command.ApplicationId = id;

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Gets the applications of a borrower")]
        [Authorize(Policy = $"{DefinedPermissions.CanReadApplications.Name}, {DefinedPermissions.CanReadOwnApplications.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpGet("borrowers/{id}/applications")]
        public async Task<IActionResult> GetApplicationsByBorrower([FromRoute] Guid id,
                                                                   [FromQuery] string? filter,
                                                                   [FromQuery] string? sortColumn,
                                                                   [FromQuery] string? sortOrder,
                                                                   [FromQuery] int pageSize,
                                                                   [FromQuery] int page) {
            var result = await _mediator.Send(new GetApplicationsByBorrowerQuery {
                BorrowerId = id,
                Filter = filter,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
                PageSize = pageSize,
                Page = page
            });
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get specific application of specific borrower")]
        [Authorize(Policy = $"{DefinedPermissions.CanReadApplications.Name}, {DefinedPermissions.CanReadOwnApplications.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpGet("borrowers/{borrowerId}/applications/{applicationId}")]
        public async Task<IActionResult> GetApplicationByBorrower([FromRoute] Guid borrowerId, [FromRoute] Guid applicationId) {
            var result = await _mediator.Send(new GetApplicationByBorrowerQuery {
                BorrowerId = borrowerId,
                ApplicationId = applicationId
            });
            return Ok(result);
        }
    }
}
