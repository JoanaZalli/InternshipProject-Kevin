using Application.Exceptions.ClientErrors;
using Application.Interfaces.Pagination;
using Application.Persistance;
using Application.UseCases.ApplicationJourney.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.UseCases.ApplicationJourney.Queries {

    public class GetApplicationsByBorrowerQuery : IRequest<PagedList<ApplicationQueryResult>> {
        public Guid BorrowerId { get; set; }
        public string? Filter { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }

    public class GetApplicationsByBorrowerQueryHandler : IRequestHandler<GetApplicationsByBorrowerQuery, PagedList<ApplicationQueryResult>> {

        private readonly IApplicationRepository _applicationRepository;
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly IMapper _mapper;
        private readonly IPaginationService<ApplicationEntity> _pagination;
        private readonly ILogger<GetApplicationsByBorrowerQueryHandler> _logger;


        public GetApplicationsByBorrowerQueryHandler(IApplicationRepository applicationRepository, IBorrowerRepository borrowerRepository, IStringLocalizer<LocalizationResources> localization, IMapper mapper, IPaginationService<ApplicationEntity> pagination, ILogger<GetApplicationsByBorrowerQueryHandler> logger) {
            _applicationRepository = applicationRepository;
            _borrowerRepository = borrowerRepository;
            _localization = localization;
            _mapper = mapper;
            _pagination = pagination;
            _logger = logger;
        }

        public async Task<PagedList<ApplicationQueryResult>> Handle(GetApplicationsByBorrowerQuery request, CancellationToken cancellationToken) {

            try {
                if (await _borrowerRepository.ContainsAsync(request.BorrowerId) is false)
                    throw new NotFoundException(_localization.GetString("BorrowerDoesntExist").Value);

                if (await _borrowerRepository.HasApplicationsAsync(request.BorrowerId) is false)
                    return new PagedList<ApplicationQueryResult>();

                var applications = _applicationRepository.GetIQueryable(request.BorrowerId);

                var tuple = _pagination.Validate(request.Page, request.PageSize, applications.Count());
                request.Page = tuple.Item1;
                request.PageSize = tuple.Item2;

                applications = Filter(applications, request.Filter);
                applications = Sort(applications, request.SortOrder, request.SortColumn);
                var response = await _pagination.PaginateAsync(applications, request.Page, request.PageSize);

                return _mapper.Map<PagedList<ApplicationQueryResult>>(response);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get Applications By Borrower Query Handler", request);

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

    public class GetApplicationsByBorrowerQueryValidator : AbstractValidator<GetApplicationsByBorrowerQuery> {
        public GetApplicationsByBorrowerQueryValidator() {
            RuleFor(x => x.BorrowerId)
                .NotEmpty().WithMessage("EmptyBorrowerId");
        }
    }
}
