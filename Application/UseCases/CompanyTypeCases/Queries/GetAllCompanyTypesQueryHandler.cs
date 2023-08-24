using Application.Interfaces.Pagination;
using Application.Persistance;
using Application.UseCases.CompanyTypeCases.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.UseCases.CompanyTypeCases.Queries {

    public class GetAllCompanyTypesQuery : IRequest<PagedList<CompanyTypeResult>> {
        public string? Filter { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }

    public class GetAllCompanyTypesQueryHandler : IRequestHandler<GetAllCompanyTypesQuery, PagedList<CompanyTypeResult>> {

        private readonly ICompanyTypeRepository _companyTypeRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService<CompanyType> _pagination;
        private readonly ILogger<GetAllCompanyTypesQueryHandler> _logger;


        public GetAllCompanyTypesQueryHandler(ICompanyTypeRepository companyTypeRepository,
                                              IMapper mapper,
                                              IPaginationService<CompanyType> pagination,
                                              ILogger<GetAllCompanyTypesQueryHandler> logger) {
            _companyTypeRepository = companyTypeRepository;
            _mapper = mapper;
            _pagination = pagination;
            _logger = logger;
        }

        public async Task<PagedList<CompanyTypeResult>> Handle(GetAllCompanyTypesQuery request, CancellationToken cancellationToken) {

            try {
                var companyTypes = _companyTypeRepository.GetIQueryable();

                var tuple = _pagination.Validate(request.Page, request.PageSize, companyTypes.Count());
                request.Page = tuple.Item1;
                request.PageSize = tuple.Item2;

                companyTypes = Filter(companyTypes, request.Filter);
                companyTypes = Sort(companyTypes, request.SortOrder, request.SortColumn);
                var response = await _pagination.PaginateAsync(companyTypes, request.Page, request.PageSize);

                return _mapper.Map<PagedList<CompanyTypeResult>>(response);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get All Company Types Query Handler", request);

                throw;
            }
        }

        private static IQueryable<CompanyType> Filter(IQueryable<CompanyType> companyTypes, string? filter) {
            if (string.IsNullOrWhiteSpace(filter) is false)
                companyTypes = companyTypes.Where(x =>
                    x.Type.Contains(filter));

            return companyTypes;
        }

        private static IQueryable<CompanyType> Sort(IQueryable<CompanyType> companyTypes, string? sortOrder, string? sortColumn) {

            Expression<Func<CompanyType, object>> key = sortColumn?.ToLower() switch {
                "type" => x => x.Type,
                _ => x => x.Id
            };

            if (sortOrder?.ToLower() == "desc")
                return companyTypes.OrderByDescending(key);
            else
                return companyTypes.OrderBy(key);
        }
    }

    public class GetAllCompanyTypesQueryValidator : AbstractValidator<GetAllCompanyTypesQuery> {
        public GetAllCompanyTypesQueryValidator() {

        }
    }
}
