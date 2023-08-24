using Application.UseCases.StatusCases.Queries;
using Domain.Seeds;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InternshipProject.Controllers {

    [ApiController]
    [Route("api")]
    public class StatusController : ControllerBase {

        private readonly IMediator _mediator;

        public StatusController(IMediator mediator) {
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Get loan statuses")]
        [Authorize(Policy = $"{DefinedPermissions.CanReadStatuses.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpGet("loan-statuses")]
        public async Task<IActionResult> GetStatuses([FromQuery] string? filter,
                                                     [FromQuery] string? sortColumn,
                                                     [FromQuery] string? sortOrder,
                                                     [FromQuery] int pageSize,
                                                     [FromQuery] int page) {
            var result = await _mediator.Send(new GetLoanStatusesQuery {
                Filter = filter,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
                PageSize = pageSize,
                Page = page
            });
            return Ok(result);
        }
    }
}
