using Application.Interfaces.Pagination;
using Application.Persistance;
using Application.UseCases.LoanJourney.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.UseCases.LoanJourney.Queries {

    public class GetAllLoansQuery : IRequest<PagedList<LoanResult>> {
        public string? Filter { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }

    public class GetAllLoansQueryHandler : IRequestHandler<GetAllLoansQuery, PagedList<LoanResult>> {

        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService<Loan> _pagination;
        private readonly ILogger<GetAllLoansQueryHandler> _logger;

        public GetAllLoansQueryHandler(ILoanRepository loanRepository,
                                       IMapper mapper,
                                       IPaginationService<Loan> pagination,
                                       ILogger<GetAllLoansQueryHandler> logger) {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _pagination = pagination;
            _logger = logger;
        }

        public async Task<PagedList<LoanResult>> Handle(GetAllLoansQuery request, CancellationToken cancellationToken) {

            try {
                var loans = _loanRepository.GetIQueryable();

                var tuple = _pagination.Validate(request.Page, request.PageSize, loans.Count());
                request.Page = tuple.Item1;
                request.PageSize = tuple.Item2;

                loans = Filter(loans, request.Filter);
                loans = Sort(loans, request.SortOrder, request.SortColumn);
                var response = await _pagination.PaginateAsync(loans, request.Page, request.PageSize);

                return _mapper.Map<PagedList<LoanResult>>(response);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get All Loans Query Handler", request);

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

    public class GetAllLoansQueryValidator : AbstractValidator<GetAllLoansQuery> {
        public GetAllLoansQueryValidator() {
        }
    }
}
