using Application.Exceptions.ClientErrors;
using Application.Persistance;
using Application.UseCases.LenderCases.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.LenderCases.Queries {

    public class GetEligibleLendersQuery : IRequest<List<LenderQueryResult>> {
        public Guid ApplicationId { get; set; }
    }

    public class GetEligibleLendersQueryHandler : IRequestHandler<GetEligibleLendersQuery, List<LenderQueryResult>> {

        private readonly ILenderRepository _lenderRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEligibleLendersQueryHandler> _logger;


        public GetEligibleLendersQueryHandler(IApplicationRepository applicationRepository,
                                              ILenderRepository lenderRepository,
                                              IStringLocalizer<LocalizationResources> localization,
                                              IMapper mapper,
                                              ILogger<GetEligibleLendersQueryHandler> logger) {
            _applicationRepository = applicationRepository;
            _lenderRepository = lenderRepository;
            _localization = localization;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<LenderQueryResult>> Handle(GetEligibleLendersQuery request, CancellationToken cancellationToken) {

            try {
                if (await _applicationRepository.ContainsAsync(request.ApplicationId) is false)
                    throw new NotFoundException(_localization.GetString("ApplicationDoesntExist").Value);

                var application = await _applicationRepository.GetByIdAsync(request.ApplicationId);
                var companyType = await _applicationRepository.GetCompanyTypeAsync(request.ApplicationId);

                var lenders = await _lenderRepository.GetAllAsync();

                var eligibles = new List<Lender>();
                foreach (var lender in lenders) {
                    if (IsTenorAccepted(lender, application.RequestedTenor) is false)
                        continue;

                    if (IsRequestAmountAccepted(lender, application.RequestedAmount) is false)
                        continue;

                    if (IsCompanyTypeAccepted(lender, companyType) is false)
                        continue;

                    eligibles.Add(lender);
                }

                if (eligibles.Any() is false)
                    throw new NotFoundException(_localization.GetString("NoEligibleLendersFound").Value);

                return _mapper.Map<List<LenderQueryResult>>(eligibles);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get Eligible Lenders Query Handler", request);

                throw;
            }
        }

        // should be private, made public for testing
        public static bool IsTenorAccepted(Lender lender, int tenor) {
            if (lender.MinTenor <= tenor && lender.MaxTenor >= tenor)
                return true;
            return false;
        }

        // should be private, made public for testing
        public static bool IsRequestAmountAccepted(Lender lender, int amount) {
            return lender.RequestedAmount < amount;
        }

        // should be private, made public for testing
        public static bool IsCompanyTypeAccepted(Lender lender, string type) {
            if (string.IsNullOrEmpty(lender.BorrowerCompanyType) is true)
                return true;

            return lender.BorrowerCompanyType == type;
        }
    }

    public class GetEligibleLendersQueryValidator : AbstractValidator<GetEligibleLendersQuery> {
        public GetEligibleLendersQueryValidator() {
            RuleFor(x => x.ApplicationId)
                .NotEmpty().WithMessage("EmptyApplicationId");
        }
    }
}
