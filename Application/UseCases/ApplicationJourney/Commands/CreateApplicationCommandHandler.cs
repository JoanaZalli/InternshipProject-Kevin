using Application.Exceptions.ClientErrors;
using Application.Exceptions.ServerErrors;
using Application.Persistance;
using Application.Persistance.Common;
using Application.UseCases.ApplicationJourney.Results;
using AutoMapper;
using Domain.Entities;
using Domain.Seeds;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ApplicationJourney.Commands {

    public class CreateApplicationCommand : IRequest<ApplicationCommandResult> {
        public Guid BorrowerId { get; set; }
        public int RequestedAmount { get; set; }
        public int RequestedTenor { get; set; }
        public string FinancePurposeDefinition { get; set; } = null!;
    }

    public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, ApplicationCommandResult> {

        private readonly IApplicationRepository _applicationRepository;
        private readonly IStringLocalizer<LocalizationResources> _localizer;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly IApplicationStatusRepository _applicationStatusRepository;
        private readonly ILogger<CreateApplicationCommandHandler> _logger;


        public CreateApplicationCommandHandler(IStringLocalizer<LocalizationResources> localizer,
                                               IApplicationRepository applicationRepository,
                                               IUnitOfWork unitOfWork,
                                               IMapper mapper,
                                               IProductRepository productRepository,
                                               IBorrowerRepository borrowerRepository,
                                               IApplicationStatusRepository applicationStatusRepository,
                                               ILogger<CreateApplicationCommandHandler> logger) {
            _localizer = localizer;
            _applicationRepository = applicationRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepository = productRepository;
            _borrowerRepository = borrowerRepository;
            _applicationStatusRepository = applicationStatusRepository;
            _logger = logger;
        }

        public async Task<ApplicationCommandResult> Handle(CreateApplicationCommand request,
                                                           CancellationToken cancellationToken) {

            try {
                // predefined can be changed later by loan offc.
                var product = await _productRepository.GetByIdAsync(DefinedProducts.FixedRatePreAmortization.Id);

                if (product.FinanceMaxAmount < request.RequestedAmount)
                    throw new InvalidRequestException(_localizer.GetString("BiggerRequestAmount").Value);
                else if (product.FinanceMinAmount > request.RequestedAmount)
                    throw new InvalidRequestException(_localizer.GetString("SmallerRequestAmount").Value);

                if (await _borrowerRepository.ContainsAsync(request.BorrowerId) is false)
                    throw new NotFoundException(_localizer.GetString("BorrowerDoesntExist").Value);

                var application = _mapper.Map<ApplicationEntity>(request);
                application.Id = Guid.NewGuid();
                application.BorrowerId = request.BorrowerId;
                application.Name = GenerateName(request.RequestedAmount);
                application.ApplicationStatus = await _applicationStatusRepository.GetByIdAsync(DefinedApplicationStatuses.InCharge.Id);
                application.Product = product;

                await _applicationRepository.CreateAsync(application);

                var flag = await _unitOfWork.SaveChangesAsync();
                if (flag is false)
                    throw new DatabaseException(_localizer.GetString("DatabaseException").Value);

                return _mapper.Map<ApplicationCommandResult>(application);
            }
            catch (Exception ex) {
                _logger.LogError("Error during application creation.", request);

                throw;
            }
        }

        private static string GenerateName(int amount) {
            return amount.ToString() + " - " + DateTime.UtcNow.ToString("d");
        }
    }

    public class CreateApplicationCommandValidator : AbstractValidator<CreateApplicationCommand> {
        public CreateApplicationCommandValidator() {
            RuleFor(x => x.BorrowerId)
                .NotEmpty().WithMessage("EmptyBorrowerId");

            RuleFor(x => x.RequestedAmount)
                .NotEmpty().WithMessage("EmptyRequestAmount");

            RuleFor(x => x.RequestedTenor)
                .NotEmpty().WithMessage("EmptyRequestTenor");

            RuleFor(x => x.RequestedTenor <= DefinedTenors.MaximumTenor && x.RequestedTenor >= DefinedTenors.MinimumTenor)
                .NotEmpty().WithMessage("TenorConstraint");

            RuleFor(x => x.FinancePurposeDefinition)
                .NotEmpty().WithMessage("EmptyFinancePurposeDefinition")
                .MaximumLength(100).WithMessage("FinancePurposeMaximumLengthRestriction");
        }
    }
}
