using Application.UseCases.LenderMatrixCases.Commands;
using Application.UseCases.LenderMatrixCases.Queries;
using Domain.Seeds;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InternshipProject.Controllers {

    [ApiController]
    [Route("api")]
    public class LenderMatrixController : ControllerBase {

        private readonly IMediator _mediator;

        public LenderMatrixController(IMediator mediator) {
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Create lender matrix")]
        [Authorize(Policy = $"{DefinedPermissions.CanUpdateMatrix.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpPost("lender-matrix")]
        public async Task<IActionResult> CreateLenderMatrix([FromQuery] Guid lenderId, [FromQuery] Guid productId, IFormFile file) {
            var flag = await _mediator.Send(new CreateLenderMatrixCommand {
                LenderId = lenderId,
                ProductId = productId,
                File = file
            });

            return Ok(flag);
        }

        [SwaggerOperation(Summary = "Generate lender matrix template")]
        [Authorize(Policy = $"{DefinedPermissions.CanGenerateMatrix.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpGet("lender-matrix/generate")]
        public async Task<IActionResult> GenerateLenderMatrix([FromQuery] Guid lenderId, [FromQuery] Guid productId, [FromQuery] bool isFilled) {

            return await _mediator.Send(new GenerateLenderMatrixQuery {
                LenderId = lenderId,
                ProductId = productId,
                IsFilled = isFilled
            });
        }

        [SwaggerOperation(Summary = "Update lender matrix")]
        [Authorize(Policy = $"{DefinedPermissions.CanUpdateMatrix.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpPut("lender-matrix")]
        public async Task<IActionResult> UpdateLenderMatrix([FromQuery] Guid lenderId, [FromQuery] Guid productId, IFormFile file) {

            var flag = await _mediator.Send(new UpdateLenderMatrixCommand {
                LenderId = lenderId,
                ProductId = productId,
                File = file
            });

            return Ok(flag);
        }

        [SwaggerOperation(Summary = "Delete lender matrix")]
        [Authorize(Policy = $"{DefinedPermissions.CanDeleteMatrix.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpDelete("lender-matrix")]
        public async Task<IActionResult> DeleteLenderMatrix([FromQuery] Guid lenderId, [FromQuery] Guid productId) {

            var flag = await _mediator.Send(new DeleteLenderMatrixCommand {
                LenderId = lenderId,
                ProductId = productId
            });

            return Ok(flag);
        }
    }
}
