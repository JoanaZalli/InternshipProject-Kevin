using Application.UseCases.CompanyTypeCases.Queries;
using Domain.Seeds;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InternshipProject.Controllers {

    [ApiController]
    [Route("api")]
    public class CompanyTypeController : ControllerBase {

        private readonly IMediator _mediator;

        public CompanyTypeController(IMediator mediator) {
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Get company types")]
        [Authorize(Policy = $"{DefinedPermissions.CanReadCompanyTypes.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpGet("company-types")]
        public async Task<IActionResult> GetCompanyTypes([FromQuery] string? filter,
                                                         [FromQuery] string? sortColumn,
                                                         [FromQuery] string? sortOrder,
                                                         [FromQuery] int pageSize,
                                                         [FromQuery] int page) {
            var result = await _mediator.Send(new GetAllCompanyTypesQuery {
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
