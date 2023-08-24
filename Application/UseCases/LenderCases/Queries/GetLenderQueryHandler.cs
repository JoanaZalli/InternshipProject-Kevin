using Application.Exceptions.ClientErrors;
using Application.Persistance;
using Application.UseCases.LenderCases.Results;
using AutoMapper;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.LenderCases.Queries {
    public class GetLendersQuery : IRequest<LenderQueryResult> {
        public Guid LenderId { get; set; }
    }

    public class GetLendersQueryHandler : IRequestHandler<GetLendersQuery, LenderQueryResult> {

        private readonly IMapper _mapper;
        private readonly ILenderRepository _lenderRepository;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly ILogger<GetLendersQueryHandler> _logger;

        public GetLendersQueryHandler(ILenderRepository lenderRepository,
                                      IMapper mapper,
                                      IStringLocalizer<LocalizationResources> localization,
                                      ILogger<GetLendersQueryHandler> logger) {
            _lenderRepository = lenderRepository;
            _mapper = mapper;
            _localization = localization;
            _logger = logger;
        }

        public async Task<LenderQueryResult> Handle(GetLendersQuery request, CancellationToken cancellationToken) {

            try {
                if (await _lenderRepository.ContainsAsync(request.LenderId) is false)
                    throw new NotFoundException(_localization.GetString("LenderDoesntExist").Value);

                var lender = await _lenderRepository.GetByIdAsync(request.LenderId);
                return _mapper.Map<LenderQueryResult>(lender);
            }
            catch (Exception ex) {
                _logger.LogError("Errors in Get Lenders Query Handler", request);

                throw;
            }
        }

        public class GetLendersQueryValidator : AbstractValidator<GetLendersQuery> {
            public GetLendersQueryValidator() {
                RuleFor(x => x.LenderId)
                    .NotEmpty().WithMessage("EmptyLenderId");
            }
        }
    }
}
