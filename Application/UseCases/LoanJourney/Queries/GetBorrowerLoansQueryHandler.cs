using Application.Exceptions.ClientErrors;
using Application.Interfaces.Pagination;
using Application.Persistance;
using Application.UseCases.LoanJourney.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.UseCases.LoanJourney.Queries {

    public class GetBorrowerLoansQuery : IRequest<PagedList<LoanResult>> {
        public Guid BorrowerId { get; set; }
        public string? Filter { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }

    public class GetBorrowerLoansQueryHandler : IRequestHandler<GetBorrowerLoansQuery, PagedList<LoanResult>> {

        private readonly IBorrowerRepository _borrowerRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService<Loan> _pagination;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly ILogger<GetBorrowerLoansQueryHandler> _logger;

        public GetBorrowerLoansQueryHandler(ILoanRepository loanRepository,
                                            IBorrowerRepository borrowerRepository,
                                            IMapper mapper,
                                            IPaginationService<Loan> pagination,
                                            IStringLocalizer<LocalizationResources> localization,
                                            ILogger<GetBorrowerLoansQueryHandler> logger) {
            _loanRepository = loanRepository;
            _borrowerRepository = borrowerRepository;
            _mapper = mapper;
            _pagination = pagination;
            _localization = localization;
            _logger = logger;
        }

        public async Task<PagedList<LoanResult>> Handle(GetBorrowerLoansQuery request, CancellationToken cancellationToken) {

            try {
                if (await _borrowerRepository.ContainsAsync(request.BorrowerId) is false)
                    throw new NotFoundException(_localization.GetString("BorrowerDoesntExist").Value);

                if (await _borrowerRepository.HasApplicationsAsync(request.BorrowerId) is false)
                    return new PagedList<LoanResult>();

                var loans = _loanRepository.GetLoansByBorrower(request.BorrowerId);

                var tuple = _pagination.Validate(request.Page, request.PageSize, loans.Count());
                request.Page = tuple.Item1;
                request.PageSize = tuple.Item2;

                loans = Filter(loans, request.Filter);
                loans = Sort(loans, request.SortOrder, request.SortColumn);
                var response = await _pagination.PaginateAsync(loans, request.Page, request.PageSize);

                return _mapper.Map<PagedList<LoanResult>>(response);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get Borrower Loans Query Handler", request);

                throw;
            }
        }

        private static IQueryable<Loan> Filter(IQueryable<Loan> loans, string? filter) {
            if (string.IsNullOrWhiteSpace(filter) is false)
                loans = loans.Where(x =>
                    x.RequestedAmount.ToString().Contains(filter) ||
                    x.ReferenceRate.ToString().Contains(filter) ||
                    x.InterestRate.ToString().Contains(filter) ||
                    x.Tenor.ToString().Contains(filter));

            return loans;
        }

        private static IQueryable<Loan> Sort(IQueryable<Loan> loans, string? sortOrder, string? sortColumn) {

            Expression<Func<Loan, object>> key = sortColumn?.ToLower() switch {
                "requestedAmount" => x => x.RequestedAmount,
                "referenceRate" => x => x.ReferenceRate,
                "interestRate" => x => x.InterestRate,
                "tenor" => x => x.Tenor,
                _ => x => x.Id
            };

            if (sortOrder?.ToLower() == "desc")
                return loans.OrderByDescending(key);
            else
                return loans.OrderBy(key);
        }
    }

    public class GetBorrowerLoansQueryValidator : AbstractValidator<GetBorrowerLoansQuery> {
        public GetBorrowerLoansQueryValidator() {
            RuleFor(x => x.BorrowerId)
                .NotEmpty().WithMessage("EmptyBorrowerId");
        }
    }
}
