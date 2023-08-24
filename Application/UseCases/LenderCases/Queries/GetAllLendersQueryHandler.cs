using Application.Interfaces.Pagination;
using Application.Persistance;
using Application.UseCases.LenderCases.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.UseCases.LenderCases.Queries {

    public class GetAllLendersQuery : IRequest<PagedList<LenderQueryResult>> {
        public string? Filter { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }

    public class GetAllLendersQueryHandler : IRequestHandler<GetAllLendersQuery, PagedList<LenderQueryResult>> {

        private readonly IMapper _mapper;
        private readonly ILenderRepository _lenderRepository;
        private readonly IPaginationService<Lender> _pagination;
        private readonly ILogger<GetAllLendersQueryHandler> _logger;

        public GetAllLendersQueryHandler(ILenderRepository lenderRepository,
                                         IMapper mapper,
                                         IPaginationService<Lender> pagination,
                                         ILogger<GetAllLendersQueryHandler> logger) {
            _lenderRepository = lenderRepository;
            _mapper = mapper;
            _pagination = pagination;
            _logger = logger;
        }

        public async Task<PagedList<LenderQueryResult>> Handle(GetAllLendersQuery request, CancellationToken cancellationToken) {

            try {
                var lenders = _lenderRepository.GetIQueryable();

                var tuple = _pagination.Validate(request.Page, request.PageSize, lenders.Count());
                request.Page = tuple.Item1;
                request.PageSize = tuple.Item2;

                lenders = Filter(lenders, request.Filter);
                lenders = Sort(lenders, request.SortOrder, request.SortColumn);
                var response = await _pagination.PaginateAsync(lenders, request.Page, request.PageSize);

                return _mapper.Map<PagedList<LenderQueryResult>>(response);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get All Lenders Query Handler", request);

                throw;
            }
        }

        private static IQueryable<Lender> Filter(IQueryable<Lender> lenders, string? filter) {
            if (string.IsNullOrWhiteSpace(filter) is false)
                lenders = lenders.Where(x =>
                    x.Name.Contains(filter) ||
                    x.RequestedAmount.ToString().Contains(filter) ||
                    x.MinTenor.ToString().Contains(filter) ||
                    x.MaxTenor.ToString().Contains(filter) ||
                    x.BorrowerCompanyType.Contains(filter));

            return lenders;
        }

        private static IQueryable<Lender> Sort(IQueryable<Lender> lenders, string? sortOrder, string? sortColumn) {

            Expression<Func<Lender, object>> key = sortColumn?.ToLower() switch {
                "name" => x => x.Name,
                "requestedAmount" => x => x.RequestedAmount,
                "minTenor" => x => x.MinTenor,
                "maxTenor" => x => x.MaxTenor,
                "borrowerCompanyType" => x => x.BorrowerCompanyType,
                _ => x => x.Id
            };

            if (sortOrder?.ToLower() == "desc")
                return lenders.OrderByDescending(key);
            else
                return lenders.OrderBy(key);
        }
    }

    public class GetAllLendersQueryValidator : AbstractValidator<GetAllLendersQuery> {
        public GetAllLendersQueryValidator() {
        }
    }
}
