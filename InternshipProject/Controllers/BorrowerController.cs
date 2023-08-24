using Application.UseCases.BorrowerJourney.Commands;
using Application.UseCases.BorrowerJourney.Queries;
using AutoMapper;
using Domain.Seeds;
using InternshipProject.Objects.Requests.BorrowerJourneyRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InternshipProject.Controllers {

    [ApiController]
    [Route("api")]
    public class BorrowerController : ControllerBase {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BorrowerController(IMapper mapper, IMediator mediator) {
            _mapper = mapper;
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Create new borrower")]
        [Authorize(Policy = $"{DefinedPermissions.CanAddBorrower.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpPost("borrowers")]
        public async Task<IActionResult> CreateBorrower([FromHeader] string AccessToken, [FromBody] BorrowerRequest borrowerRequest) {
            var command = _mapper.Map<CreateBorrowerCommmand>(borrowerRequest);
            command.AccessToken = AccessToken;

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get borrower by id")]
        [Authorize(Policy = $"{DefinedPermissions.CanReadBorrowers.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpGet("borrowers/{id}")]
        public async Task<IActionResult> GetBorrower([FromRoute] Guid id) {
            var result = await _mediator.Send(new GetBorrowerQuery {
                BorrowerId = id
            });
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Update borrower")]
        [Authorize(Policy = $"{DefinedPermissions.CanUpdateBorrower.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpPut("borrowers/{id}")]
        public async Task<IActionResult> UpdateBorrower([FromRoute] Guid id, [FromBody] BorrowerRequest borrowerRequest) {
            var command = _mapper.Map<UpdateBorrowerCommand>(borrowerRequest);
            command.BorrowerId = id;

            _ = await _mediator.Send(command);
            return Ok();
        }

        [SwaggerOperation(Summary = "Get borrowers of a user")]
        [Authorize(Policy = $"{DefinedPermissions.CanReadBorrowers.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpGet("users/{id}/borrowers")]
        public async Task<IActionResult> GetBorrowersByUser([FromRoute] Guid id,
                                                            [FromQuery] string? filter,
                                                            [FromQuery] string? sortColumn,
                                                            [FromQuery] string? sortOrder,
                                                            [FromQuery] int pageSize,
                                                            [FromQuery] int page) {

            var borrowers = await _mediator.Send(new GetUserBorrowersQuery {
                UserId = id,
                Filter = filter,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
                PageSize = pageSize,
                Page = page
            });

            return Ok(borrowers);
        }

        [SwaggerOperation(Summary = "Get all borrowers")]
        [Authorize(Policy = $"{DefinedPermissions.CanReadBorrowers.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpGet("borrowers")]
        public async Task<IActionResult> GetAllBorrowers([FromQuery] string? filter,
                                                         [FromQuery] string? sortColumn,
                                                         [FromQuery] string? sortOrder,
                                                         [FromQuery] int pageSize,
                                                         [FromQuery] int page) {

            var borrowers = await _mediator.Send(new GetBorrowersQuery {
                Filter = filter,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
                PageSize = pageSize,
                Page = page
            });

            return Ok(borrowers);
        }

        [SwaggerOperation(Summary = "Get the company profile of a borrower")]
        [Authorize(Policy = $"{DefinedPermissions.CanReadBorrowers.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpGet("borrowers/{id}/company-profile")]
        public async Task<IActionResult> GetCompanyProfile([FromRoute] Guid id) {

            var companyProfile = await _mediator.Send(new GetCompanyProfileQuery {
                BorrowerId = id,
            });

            return Ok(companyProfile);
        }
    }
}
