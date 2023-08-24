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

    public class GetApplicationQuery : IRequest<ApplicationQueryResult> {
        public Guid ApplicationId { get; set; }
    }

    public class GetApplicationQueryHandler : IRequestHandler<GetApplicationQuery, ApplicationQueryResult> {

        private readonly IApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly ILogger<GetApplicationQueryHandler> _logger;


        public GetApplicationQueryHandler(IMapper mapper, IApplicationRepository applicationRepository, IStringLocalizer<LocalizationResources> localization, ILogger<GetApplicationQueryHandler> logger) {
            _mapper = mapper;
            _applicationRepository = applicationRepository;
            _localization = localization;
            _logger = logger;
        }

        public async Task<ApplicationQueryResult> Handle(GetApplicationQuery request, CancellationToken cancellationToken) {

            try {
                if (await _applicationRepository.ContainsAsync(request.ApplicationId) is false)
                    throw new NotFoundException(_localization.GetString("ApplicationDoesntExist").Value);

                var application = await _applicationRepository.GetByIdAsync(request.ApplicationId);

                return _mapper.Map<ApplicationQueryResult>(application);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get Application Query Handler", request);

                throw;
            }
        }

    }

    public class GetApplicationQueryValidator : AbstractValidator<GetApplicationQuery> {
        public GetApplicationQueryValidator() {
            RuleFor(x => x.ApplicationId)
                .NotEmpty().WithMessage("EmptyApplicationId");
        }
    }
}
