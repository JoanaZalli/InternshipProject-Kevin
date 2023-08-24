using Application.Exceptions.ClientErrors;
using Application.Persistance;
using Application.UseCases.ApplicationJourney.Results;
using AutoMapper;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ApplicationJourney.Queries {

    public class GetApplicationByBorrowerQuery : IRequest<ApplicationQueryResult> {
        public Guid BorrowerId { get; set; }
        public Guid ApplicationId { get; set; }
    }

    public class GetApplicationByBorrowerQueryHandler : IRequestHandler<GetApplicationByBorrowerQuery, ApplicationQueryResult> {

        private readonly IApplicationRepository _applicationRepository;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetApplicationByBorrowerQueryHandler> _logger;


        public GetApplicationByBorrowerQueryHandler(IMapper mapper,
                                                    IStringLocalizer<LocalizationResources> localization,
                                                    IApplicationRepository applicationRepository,
                                                    IBorrowerRepository borrowerRepository,
                                                    ILogger<GetApplicationByBorrowerQueryHandler> logger) {
            _mapper = mapper;
            _localization = localization;
            _applicationRepository = applicationRepository;
            _borrowerRepository = borrowerRepository;
            _logger = logger;
        }

        public async Task<ApplicationQueryResult> Handle(GetApplicationByBorrowerQuery request, CancellationToken cancellationToken) {
            try {
                if (await _borrowerRepository.ContainsAsync(request.BorrowerId) is false)
                    throw new NotFoundException(_localization.GetString("BorrowerDoesntExist").Value);

                if (await _applicationRepository.ContainsAsync(request.ApplicationId) is false)
                    throw new NotFoundException(_localization.GetString("ApplicationDoesntExist").Value);

                var application = await _applicationRepository.GetApplicationByBorrowerAsync(request.BorrowerId, request.ApplicationId);

                return _mapper.Map<ApplicationQueryResult>(application);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get Application By Borrower Query Handler", request);

                throw;
            }
        }
    }

    public class GetApplicationByBorrowerQueryValidator : AbstractValidator<GetApplicationByBorrowerQuery> {
        public GetApplicationByBorrowerQueryValidator() {
            RuleFor(x => x.BorrowerId)
                .NotEmpty().WithMessage("EmptyBorrowerId");
            RuleFor(x => x.ApplicationId)
                .NotEmpty().WithMessage("EmptyApplicationId");
        }
    }
}
