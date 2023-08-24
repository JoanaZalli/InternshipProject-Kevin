using Application.Interfaces.Pagination;
using Application.Persistance;
using Application.UseCases.ApplicationJourney.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.UseCases.ApplicationJourney.Queries {

    public class GetAllApplicationsQuery : IRequest<PagedList<ApplicationQueryResult>> {
        public string? Filter { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }

    public class GetAllApplicationsQueryHandler : IRequestHandler<GetAllApplicationsQuery, PagedList<ApplicationQueryResult>> {

        private readonly IApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService<ApplicationEntity> _pagination;
        private readonly ILogger<GetAllApplicationsQueryHandler> _logger;


        public GetAllApplicationsQueryHandler(IPaginationService<ApplicationEntity> pagination, IMapper mapper, IApplicationRepository applicationRepository, ILogger<GetAllApplicationsQueryHandler> logger) {
            _pagination = pagination;
            _mapper = mapper;
            _applicationRepository = applicationRepository;
            _logger = logger;
        }

        public async Task<PagedList<ApplicationQueryResult>> Handle(GetAllApplicationsQuery request, CancellationToken cancellationToken) {
            try {
                var applications = _applicationRepository.GetIQueryable();

                var tuple = _pagination.Validate(request.Page, request.PageSize, applications.Count());
                request.Page = tuple.Item1;
                request.PageSize = tuple.Item2;

                applications = Filter(applications, request.Filter);
                applications = Sort(applications, request.SortOrder, request.SortColumn);
                var response = await _pagination.PaginateAsync(applications, request.Page, request.PageSize);

                return _mapper.Map<PagedList<ApplicationQueryResult>>(response);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get All Applications Query Handler.", request);

                throw;
            }
        }

        private static IQueryable<ApplicationEntity> Filter(IQueryable<ApplicationEntity> applications, string? filter) {
            if (string.IsNullOrWhiteSpace(filter) is false)
                applications = applications.Where(x =>
                    x.Name.Contains(filter) ||
                    x.RequestedAmount.ToString().Contains(filter) ||
                    x.RequestedTenor.ToString().Contains(filter));

            return applications;
        }

        private static IQueryable<ApplicationEntity> Sort(IQueryable<ApplicationEntity> applications, string? sortOrder, string? sortColumn) {

            Expression<Func<ApplicationEntity, object>> key = sortColumn?.ToLower() switch {
                "name" => x => x.Name,
                "requestedAmount" => x => x.RequestedAmount,
                "requestedTenor" => x => x.RequestedTenor,
                _ => x => x.Id
            };

            if (sortOrder?.ToLower() == "desc")
                return applications.OrderByDescending(key);
            else
                return applications.OrderBy(key);
        }
    }

    public class GetAllApplicationsQueryValidator : AbstractValidator<GetAllApplicationsQuery> {
        public GetAllApplicationsQueryValidator() { }
    }
}
