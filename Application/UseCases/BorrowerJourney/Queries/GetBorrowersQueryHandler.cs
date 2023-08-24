using Application.Interfaces.Pagination;
using Application.Persistance;
using Application.UseCases.BorrowerJourney.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.UseCases.BorrowerJourney.Queries {

    public class GetBorrowersQuery : IRequest<PagedList<BorrowerQueryResult>> {
        public string? Filter { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }


    public class GetBorrowersQueryHandler : IRequestHandler<GetBorrowersQuery, PagedList<BorrowerQueryResult>> {

        private readonly IBorrowerRepository _borrowerRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService<Borrower> _pagination;
        private readonly ILogger<GetBorrowersQueryHandler> _logger;

        public GetBorrowersQueryHandler(IPaginationService<Borrower> pagination,
                                        IMapper mapper,
                                        IBorrowerRepository borrowerRepository,
                                        ILogger<GetBorrowersQueryHandler> logger) {
            _pagination = pagination;
            _mapper = mapper;
            _borrowerRepository = borrowerRepository;
            _logger = logger;
        }

        public async Task<PagedList<BorrowerQueryResult>> Handle(GetBorrowersQuery request, CancellationToken cancellationToken) {

            try {
                var borrowers = _borrowerRepository.GetIQueryable();

                var tuple = _pagination.Validate(request.Page, request.PageSize, borrowers.Count());
                request.Page = tuple.Item1;
                request.PageSize = tuple.Item2;

                borrowers = Filter(borrowers, request.Filter);
                borrowers = Sort(borrowers, request.SortOrder, request.SortColumn);
                var response = await _pagination.PaginateAsync(borrowers, request.Page, request.PageSize);

                return _mapper.Map<PagedList<BorrowerQueryResult>>(response);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get Borrowers Query Handler", request);

                throw;
            }
        }

        private static IQueryable<Borrower> Filter(IQueryable<Borrower> borrowers, string? filter) {
            if (string.IsNullOrWhiteSpace(filter) is false)
                borrowers = borrowers.Where(x =>
                    x.CompanyName.Contains(filter) ||
                    x.VATNumber.ToString().Contains(filter) ||
                    x.FiscalCode.ToString().Contains(filter));

            return borrowers;
        }

        private static IQueryable<Borrower> Sort(IQueryable<Borrower> borrowers, string? sortOrder, string? sortColumn) {

            Expression<Func<Borrower, object>> key = sortColumn?.ToLower() switch {
                "companyName" => x => x.CompanyName,
                _ => x => x.Id
            };

            if (sortOrder?.ToLower() == "desc")
                return borrowers.OrderByDescending(key);
            else
                return borrowers.OrderBy(key);
        }
    }

    public class GetBorrowersQueryValidator : AbstractValidator<GetBorrowersQuery> {
        public GetBorrowersQueryValidator() {
        }
    }
}
