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

    public class GetBorrowerLoanQuery : IRequest<LoanResult> {
        public Guid BorrowerId { get; set; }
        public Guid LoanId { get; set; }
    }

    public class GetBorrowerLoanQueryHandler : IRequestHandler<GetBorrowerLoanQuery, LoanResult> {

        private readonly ILoanRepository _loanRepository;
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly ILogger<GetBorrowerLoanQueryHandler> _logger;

        public GetBorrowerLoanQueryHandler(IMapper mapper,
                                           IBorrowerRepository borrowerRepository,
                                           ILoanRepository loanRepository,
                                           IStringLocalizer<LocalizationResources> localization,
                                           ILogger<GetBorrowerLoanQueryHandler> logger) {
            _mapper = mapper;
            _borrowerRepository = borrowerRepository;
            _loanRepository = loanRepository;
            _localization = localization;
            _logger = logger;
        }

        public async Task<LoanResult> Handle(GetBorrowerLoanQuery request, CancellationToken cancellationToken) {

            try {
                if (await _borrowerRepository.ContainsAsync(request.BorrowerId) is false)
                    throw new NotFoundException(_localization.GetString("BorrowerDoesntExist").Value);

                if (await _loanRepository.ContainsAsync(request.LoanId) is false)
                    throw new NotFoundException(_localization.GetString("LoanDoesntExist").Value);

                var loan = await _loanRepository.GetLoanByBorrowerAsync(request.BorrowerId, request.LoanId)
                           ?? throw new InvalidRequestException(_localization.GetString("DoesntBelongTo").Value);

                return _mapper.Map<LoanResult>(loan);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get Borrower Loan Query Handler", request);

                throw;
            }
        }
    }

    public class GetBorrowerLoanQueryValidator : AbstractValidator<GetBorrowerLoanQuery> {
        public GetBorrowerLoanQueryValidator() {
            RuleFor(x => x.BorrowerId)
                .NotEmpty().WithMessage("EmptyBorrowerId");

            RuleFor(x => x.LoanId)
                .NotEmpty().WithMessage("EmptyLoanId");
        }
    }
}
