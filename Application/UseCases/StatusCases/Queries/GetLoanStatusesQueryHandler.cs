using Application.Interfaces.Pagination;
using Application.Persistance;
using Application.UseCases.StatusCases.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.UseCases.StatusCases.Queries {

    public class GetLoanStatusesQuery : IRequest<PagedList<LoanStatusResult>> {
        public string? Filter { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }


    public class GetLoanStatusesQueryHandler : IRequestHandler<GetLoanStatusesQuery, PagedList<LoanStatusResult>> {

        private readonly ILoanStatusRepository _loanStatusRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService<LoanStatus> _pagination;
        private readonly ILogger<GetLoanStatusesQueryHandler> _logger;

        public GetLoanStatusesQueryHandler(ILoanStatusRepository loanStatusRepository,
                                           IMapper mapper,
                                           IPaginationService<LoanStatus> pagination,
                                           ILogger<GetLoanStatusesQueryHandler> logger) {
            _loanStatusRepository = loanStatusRepository;
            _mapper = mapper;
            _pagination = pagination;
            _logger = logger;
        }

        public async Task<PagedList<LoanStatusResult>> Handle(GetLoanStatusesQuery request, CancellationToken cancellationToken) {

            try {
                var statuses = _loanStatusRepository.GetIQueryable();

                var tuple = _pagination.Validate(request.Page, request.PageSize, statuses.Count());
                request.Page = tuple.Item1;
                request.PageSize = tuple.Item2;

                statuses = Filter(statuses, request.Filter);
                statuses = Sort(statuses, request.SortOrder, request.SortColumn);
                var response = await _pagination.PaginateAsync(statuses, request.Page, request.PageSize);

                return _mapper.Map<PagedList<LoanStatusResult>>(response);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get Loan Statuses Query Handler", request);

                throw;
            }
        }

        private static IQueryable<LoanStatus> Filter(IQueryable<LoanStatus> statuses, string? filter) {
            if (string.IsNullOrWhiteSpace(filter) is false)
                statuses = statuses.Where(x =>
                    x.Name.Contains(filter));

            return statuses;
        }

        private static IQueryable<LoanStatus> Sort(IQueryable<LoanStatus> statuses, string? sortOrder, string? sortColumn) {

            Expression<Func<LoanStatus, object>> key = sortColumn?.ToLower() switch {
                "name" => x => x.Name,
                _ => x => x.Id
            };

            if (sortOrder?.ToLower() == "desc")
                return statuses.OrderByDescending(key);
            else
                return statuses.OrderBy(key);
        }
    }

    public class GetLoanStatusesQueryValidator : AbstractValidator<GetLoanStatusesQuery> {
        public GetLoanStatusesQueryValidator() {
        }
    }
}
