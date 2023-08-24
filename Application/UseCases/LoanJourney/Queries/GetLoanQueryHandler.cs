using Application.Exceptions.ClientErrors;
using Application.Persistance;
using Application.UseCases.LoanJourney.Results;
using AutoMapper;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.LoanJourney.Queries {

    public class GetLoanQuery : IRequest<LoanResult> {
        public Guid LoanId { get; set; }
    }

    public class GetLoanQueryHandler : IRequestHandler<GetLoanQuery, LoanResult> {

        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly ILogger<GetLoanQueryHandler> _logger;

        public GetLoanQueryHandler(IMapper mapper,
                                   ILoanRepository loanRepository,
                                   IStringLocalizer<LocalizationResources> localization,
                                   ILogger<GetLoanQueryHandler> logger) {
            _mapper = mapper;
            _loanRepository = loanRepository;
            _localization = localization;
            _logger = logger;
        }

        public async Task<LoanResult> Handle(GetLoanQuery request, CancellationToken cancellationToken) {

            try {
                if (await _loanRepository.ContainsAsync(request.LoanId) is false)
                    throw new NotFoundException(_localization.GetString("LoanDoesntExist").Value);

                var loan = await _loanRepository.GetByIdAsync(request.LoanId);

                return _mapper.Map<LoanResult>(loan);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get Loan Query Handler", request);

                throw;
            }
        }
    }

    public class GetLoanQueryValidator : AbstractValidator<GetLoanQuery> {
        public GetLoanQueryValidator() {
            RuleFor(x => x.LoanId)
                .NotEmpty().WithMessage("EmptyLoanId");
        }
    }
}
