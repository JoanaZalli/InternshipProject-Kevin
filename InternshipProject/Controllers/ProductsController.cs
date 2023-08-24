using Application.UseCases.ProductCases.Queries;
using Domain.Seeds;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InternshipProject.Controllers {

    [ApiController]
    [Route("api")]
    public class ProductsController : ControllerBase {

        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator) {
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Get products")]
        [Authorize(Policy = $"{DefinedPermissions.CanReadProducts.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts([FromQuery] string? filter,
                                                     [FromQuery] string? sortColumn,
                                                     [FromQuery] string? sortOrder,
                                                     [FromQuery] int pageSize,
                                                     [FromQuery] int page) {
            var result = await _mediator.Send(new GetProductsQuery {
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
