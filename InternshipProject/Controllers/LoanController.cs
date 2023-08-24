using Application.UseCases.LoanJourney.Commands;
using Application.UseCases.LoanJourney.Queries;
using AutoMapper;
using Domain.Seeds;
using InternshipProject.Objects.Requests.LoanRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InternshipProject.Controllers {

    [ApiController]
    [Route("api")]
    public class LoanController : ControllerBase {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public LoanController(IMapper mapper, IMediator mediator) {
            _mapper = mapper;
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Approve application as loan")]
        [Authorize(Policy = $"{DefinedPermissions.CanAddLoan.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpPost("applications/{id}/loans")]
        public async Task<IActionResult> CreateLoan([FromRoute] Guid id, [FromBody] CreateLoanRequest request) {
            var command = _mapper.Map<CreateLoanCommand>(request);
            command.ApplicationId = id;

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get all loans")]
        [Authorize(Policy = $"{DefinedPermissions.CanReadLoans.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpGet("loans")]
        public async Task<IActionResult> GetAllLoans([FromQuery] string? filter,
                                                     [FromQuery] string? sortColumn,
                                                     [FromQuery] string? sortOrder,
                                                     [FromQuery] int pageSize,
                                                     [FromQuery] int page) {
            var result = await _mediator.Send(new GetAllLoansQuery {
                Filter = filter,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
                PageSize = pageSize,
                Page = page
            });

            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get loan by id")]
        [Authorize(Policy = $"{DefinedPermissions.CanReadLoans.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpGet("loans/{id}")]
        public async Task<IActionResult> GetLoanById([FromRoute] Guid id) {
            var result = await _mediator.Send(new GetLoanQuery {
                LoanId = id
            });

            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get all loans of a borrower")]
        [Authorize(Policy = $"{DefinedPermissions.CanReadLoans.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpGet("borrowers/{id}/loans")]
        public async Task<IActionResult> GetBorrowerLoans([FromRoute] Guid id,
                                                          [FromQuery] string? filter,
                                                          [FromQuery] string? sortColumn,
                                                          [FromQuery] string? sortOrder,
                                                          [FromQuery] int pageSize,
                                                          [FromQuery] int page) {
            var result = await _mediator.Send(new GetBorrowerLoansQuery {
                BorrowerId = id,
                Filter = filter,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
                PageSize = pageSize,
                Page = page
            });

            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get a specific loan of a borrower")]
        [Authorize(Policy = $"{DefinedPermissions.CanReadLoans.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpGet("borrowers/{borrowerId}/loans/{loanId}")]
        public async Task<IActionResult> GetBorrowerLoan([FromRoute] Guid borrowerId, [FromRoute] Guid loanId) {
            var result = await _mediator.Send(new GetBorrowerLoanQuery {
                LoanId = loanId,
                BorrowerId = borrowerId
            });
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Change loan status")]
        [Authorize(Policy = $"{DefinedPermissions.CanChangeLoanStatus.Name}, {DefinedPermissions.IsSuperAdmin.Name}")]
        [HttpPatch("loans/{id}/change-status")]
        public async Task<IActionResult> ChangeLoanStatus([FromRoute] Guid id, [FromBody] LoanStatusRequest request) {
            var result = await _mediator.Send(new ChangeLoanStatusCommand {
                LoanId = id,
                StatusId = request.Id
            });
            return Ok(result);
        }
    }
}
