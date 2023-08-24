using Application.Exceptions.ClientErrors;
using Application.Exceptions.ServerErrors;
using Application.Persistance;
using Application.Persistance.Common;
using Application.UseCases.LoanJourney.Results;
using Domain.Entities;
using Domain.Seeds;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.LoanJourney.Commands {

    public class CreateLoanCommand : IRequest<LoanResult> {
        public Guid ApplicationId { get; set; }
        public Guid LenderId { get; set; }
    }

    public class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, LoanResult> {

        private readonly IApplicationRepository _applicationRepository;
        private readonly ILenderRepository _lenderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly ILenderMatrixRepository _lenderMatrixRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly ILoanStatusRepository _loanStatusRepository;
        private readonly ILogger<CreateLoanCommandHandler> _logger;

        public CreateLoanCommandHandler(IStringLocalizer<LocalizationResources> localization,
                                        IUnitOfWork unitOfWork,
                                        ILenderRepository lenderRepository,
                                        IApplicationRepository applicationRepository,
                                        ILenderMatrixRepository lenderMatrixRepository,
                                        ILoanRepository loanRepository,
                                        ILoanStatusRepository loanStatusRepository,
                                        ILogger<CreateLoanCommandHandler> logger) {
            _localization = localization;
            _unitOfWork = unitOfWork;
            _lenderRepository = lenderRepository;
            _applicationRepository = applicationRepository;
            _lenderMatrixRepository = lenderMatrixRepository;
            _loanRepository = loanRepository;
            _loanStatusRepository = loanStatusRepository;
            _logger = logger;
        }

        public async Task<LoanResult> Handle(CreateLoanCommand request, CancellationToken cancellationToken) {

            try {
                if (await _applicationRepository.ContainsAsync(request.ApplicationId) is false)
                    throw new NotFoundException(_localization.GetString("ApplicationDoesntExist").Value);

                if (await _lenderRepository.ContainsAsync(request.LenderId) is false)
                    throw new NotFoundException(_localization.GetString("LenderDoesntExist").Value);

                var application = await _applicationRepository.GetWithProductAsync(request.ApplicationId);

                if (await _applicationRepository.IsApprovedAsLoanAsync(application.Id) is true)
                    throw new ConflictException(_localization.GetString("AlreadyApproved").Value);

                var lender = await _lenderRepository.GetByIdAsync(request.LenderId);

                if (await IsEligible(lender, application) is false)
                    throw new InvalidRequestException(_localization.GetString("NotEligible").Value);

                var product = application.Product;

                if (await _lenderMatrixRepository.ContainsAsync(request.LenderId, product.Id, application.RequestedTenor) is false)
                    throw new NotFoundException(_localization.GetString("MatrixDoesntExist").Value);

                var matrix = await _lenderMatrixRepository.GetByIdAsync(request.LenderId, product.Id, application.RequestedTenor);

                var loan = new Loan {
                    Id = Guid.NewGuid(),
                    RequestedAmount = application.RequestedAmount,
                    ReferenceRate = product.ReferenceRate,
                    InterestRate = matrix.Spread + product.ReferenceRate,
                    Tenor = application.RequestedTenor,
                    ProductId = product.Id,
                    Application = application,
                    Lender = lender,
                    LoanStatus = await _loanStatusRepository.GetByIdAsync(DefinedLoanStatuses.Created.Id)
                };

                await _loanRepository.CreateAsync(loan);

                var flag = await _unitOfWork.SaveChangesAsync();
                if (flag is false)
                    throw new DatabaseException(_localization.GetString("DatabaseException"));

                return new LoanResult {
                    Id = loan.Id,
                    InterestRate = loan.InterestRate,
                    ReferenceRate = loan.ReferenceRate,
                    RequestedAmount = loan.RequestedAmount,
                    Status = loan.LoanStatus.Name,
                    Tenor = loan.Tenor
                };
            }
            catch (Exception ex) {
                _logger.LogError("Error in Create Loan Command Handler", request);

                throw;
            }
        }

        // should be private, made public for testing
        public async Task<bool> IsEligible(Lender lender, ApplicationEntity application) {

            if (lender.MinTenor > application.RequestedTenor || lender.MaxTenor < application.RequestedTenor)
                return false;

            if (lender.RequestedAmount > application.RequestedAmount)
                return false;

            if ((string.IsNullOrEmpty(lender.BorrowerCompanyType) is false) && (lender.BorrowerCompanyType
                != await _applicationRepository.GetCompanyTypeAsync(application.Id)))
                return false;

            return true;
        }
    }

    public class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand> {
        public CreateLoanCommandValidator() {
            RuleFor(x => x.ApplicationId)
                .NotEmpty().WithMessage("EmptyApplicationId");

            RuleFor(x => x.LenderId)
                .NotEmpty().WithMessage("EmptyLenderId");
        }
    }
}