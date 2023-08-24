using Application.Exceptions.ClientErrors;
using Application.Exceptions.ServerErrors;
using Application.Persistance;
using Application.Persistance.Common;
using Domain.Seeds;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.LoanJourney.Commands {

    public class ChangeLoanStatusCommand : IRequest<bool> {
        public Guid LoanId { get; set; }
        public Guid StatusId { get; set; }
    }


    public class ChangeLoanStatusCommandHandler : IRequestHandler<ChangeLoanStatusCommand, bool> {

        private readonly ILoanStatusRepository _loanStatusRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly ILogger<ChangeLoanStatusCommandHandler> _logger;

        public ChangeLoanStatusCommandHandler(IUnitOfWork unitOfWork,
                                              ILoanRepository loanRepository,
                                              ILoanStatusRepository loanStatusRepository,
                                              IApplicationRepository applicationRepository,
                                              IStringLocalizer<LocalizationResources> localization,
                                              ILogger<ChangeLoanStatusCommandHandler> logger) {
            _unitOfWork = unitOfWork;
            _loanRepository = loanRepository;
            _loanStatusRepository = loanStatusRepository;
            _applicationRepository = applicationRepository;
            _localization = localization;
            _logger = logger;
        }

        public async Task<bool> Handle(ChangeLoanStatusCommand request, CancellationToken cancellationToken) {

            try {
                if (await _loanRepository.ContainsAsync(request.LoanId) is false)
                    throw new NotFoundException(_localization.GetString("LoanDoesntExist").Value);

                if (await _loanStatusRepository.ContainsAsync(request.StatusId) is false)
                    throw new NotFoundException(_localization.GetString("LoanStatusDoesntExist").Value);

                var loan = await _loanRepository.GetByIdWithApplicationAsync(request.LoanId);
                var application = loan.Application;
                var status = await _loanStatusRepository.GetByIdAsync(request.StatusId);

                switch (status.Name) {
                    case DefinedLoanStatuses.Created.Name:
                        await _applicationRepository.UpdateStatusAsync(application.Id, DefinedApplicationStatuses.InCharge.Id);
                        await _loanRepository.UpdateStatusAsync(loan.Id, DefinedLoanStatuses.Created.Id);
                        break;
                    case DefinedLoanStatuses.Erased.Name:
                        await _applicationRepository.UpdateStatusAsync(application.Id, DefinedApplicationStatuses.Canceled.Id);
                        await _loanRepository.UpdateStatusAsync(loan.Id, DefinedLoanStatuses.Erased.Id);
                        break;
                    case DefinedLoanStatuses.Defaulted.Name:
                        await _applicationRepository.UpdateStatusAsync(application.Id, DefinedApplicationStatuses.Defaulted.Id);
                        await _loanRepository.UpdateStatusAsync(loan.Id, DefinedLoanStatuses.Defaulted.Id);
                        break;
                    case DefinedLoanStatuses.Disbursed.Name:
                        await _applicationRepository.UpdateStatusAsync(application.Id, DefinedApplicationStatuses.Disbursed.Id);
                        await _loanRepository.UpdateStatusAsync(loan.Id, DefinedLoanStatuses.Disbursed.Id);
                        break;
                    case DefinedLoanStatuses.Guaranted.Name:
                        await _applicationRepository.UpdateStatusAsync(application.Id, DefinedApplicationStatuses.Guaranted.Id);
                        await _loanRepository.UpdateStatusAsync(loan.Id, DefinedLoanStatuses.Guaranted.Id);
                        break;
                    case DefinedLoanStatuses.Rejected.Name:
                        await _applicationRepository.UpdateStatusAsync(application.Id, DefinedApplicationStatuses.Rejected.Id);
                        await _loanRepository.UpdateStatusAsync(loan.Id, DefinedLoanStatuses.Rejected.Id);
                        break;
                    case DefinedLoanStatuses.Repaid.Name:
                        await _applicationRepository.UpdateStatusAsync(application.Id, DefinedApplicationStatuses.Repaid.Id);
                        await _loanRepository.UpdateStatusAsync(loan.Id, DefinedLoanStatuses.Repaid.Id);
                        break;
                    default:
                        throw new InvalidRequestException(_localization.GetString("UndefinedStatus"));
                }

                var flag = await _unitOfWork.SaveChangesAsync();
                if (flag is false)
                    throw new DatabaseException(_localization.GetString("DatabaseException"));

                return true;
            }
            catch (Exception ex) {
                _logger.LogError("Error in Change Loan Status Command Handler", request);

                throw;
            }
        }
    }

    public class ChangeLoanStatusCommandValidator : AbstractValidator<ChangeLoanStatusCommand> {
        public ChangeLoanStatusCommandValidator() {
            RuleFor(x => x.StatusId)
                .NotEmpty().WithMessage("EmptyStatusId");

            RuleFor(x => x.LoanId)
                .NotEmpty().WithMessage("EmptyLoanId");
        }
    }
}
