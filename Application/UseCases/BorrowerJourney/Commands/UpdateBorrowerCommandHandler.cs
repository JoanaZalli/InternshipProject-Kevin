using Application.Exceptions.ClientErrors;
using Application.Exceptions.ServerErrors;
using Application.Persistance;
using Application.Persistance.Common;
using Application.UseCases.Common;
using Domain.Entities;
using Domain.Seeds;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Application.UseCases.BorrowerJourney.Commands {

    public class UpdateBorrowerCommand : IRequest<bool> {
        public Guid BorrowerId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string CompanyTypeId { get; set; } = null!;
        public string VATNumber { get; set; } = null!;
        public string FiscalCode { get; set; } = null!;
    }

    public class UpdateBorrowerCommandHandler : IRequestHandler<UpdateBorrowerCommand, bool> {

        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly ICompanyTypeRepository _companyTypeRepository;
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICompanyProfileRepository _companyProfileRepository;
        private readonly FinHubConnectionSettings _finHubConnectionSettings;
        private readonly ILogger<UpdateBorrowerCommandHandler> _logger;

        public UpdateBorrowerCommandHandler(IStringLocalizer<LocalizationResources> localizer,
                                            ICompanyTypeRepository companyTypeRepository,
                                            IBorrowerRepository borrowerRepository,
                                            IUnitOfWork unitOfWork,
                                            ICompanyProfileRepository companyProfileRepository,
                                            IOptions<FinHubConnectionSettings> finHubConnectionSettings,
                                            ILogger<UpdateBorrowerCommandHandler> logger) {
            _localization = localizer;
            _companyTypeRepository = companyTypeRepository;
            _borrowerRepository = borrowerRepository;
            _unitOfWork = unitOfWork;
            _companyProfileRepository = companyProfileRepository;
            _finHubConnectionSettings = finHubConnectionSettings.Value;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateBorrowerCommand request, CancellationToken cancellationToken) {

            try {

                if (await _borrowerRepository.ContainsAsync(request.BorrowerId) is false)
                    throw new NotFoundException("BorrowerDoesntExist");

                var borrower = await _borrowerRepository.GetByIdAsync(request.BorrowerId);

                var companyTypeId = Guid.Parse(request.CompanyTypeId);

                if (await _companyTypeRepository.ContainsAsync(companyTypeId) is false)
                    throw new NotFoundException(_localization.GetString("CompanyTypeDoesntExist").Value);

                if (await _borrowerRepository.IsFiscalCodeUniqueAsync(borrower.UserId, request.FiscalCode) is false)
                    throw new ConflictException(_localization.GetString("DuplicateFiscalCode").Value);

                // validate length
                if (IsValid(companyTypeId, request.FiscalCode) is false)
                    throw new InvalidRequestException(_localization.GetString("FiscalCodeLengthRestriction").Value);

                var updateBorrower = new Borrower {
                    CompanyName = request.CompanyName,
                    VATNumber = request.VATNumber,
                    FiscalCode = request.FiscalCode,
                    CompanyType = await _companyTypeRepository.GetByIdAsync(companyTypeId),
                };

                await _companyProfileRepository.UpdateAsync(borrower.CompanyProfile.Id, await GetCompanyProfile(request.CompanyName,
                                                                                                                _finHubConnectionSettings.AuthToken));
                await _borrowerRepository.UpdateAsync(request.BorrowerId, updateBorrower);

                var flag = await _unitOfWork.SaveChangesAsync();
                if (flag is false)
                    throw new DatabaseException(_localization.GetString("DatabaseException").Value);

                return true;
            }
            catch (Exception ex) {
                _logger.LogError("Error in Update Borrower Command Handler", request);

                throw;
            }
        }

        private static async Task<CompanyProfile> GetCompanyProfile(string companyName, string authToken) {
            var client = new HttpClient {
                BaseAddress = new Uri("https://finnhub.io/api/v1/stock/")
            };

            HttpResponseMessage response;
            try {
                response = await client.GetAsync(client.BaseAddress
                    + $"profile2?symbol={companyName}&token={authToken}");
            }
            catch (Exception) {
                throw new ThirdPartyException("Incorrect authentication key.");
            }

            if (response.IsSuccessStatusCode is false)
#pragma warning disable CS8604 // Possible null reference argument.
                throw new ThirdPartyException(response.ReasonPhrase);
#pragma warning restore CS8604 // Possible null reference argument.

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CompanyProfile>(stringResponse,
                                                              new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            return result is null ? throw new ThirdPartyException("Invalid JSON data.") : result;
        }

        private static bool IsValid(Guid id, string fiscalCode) {
            if (id == DefinedCompanyTypes.SoleProprietorship.Id)
                return fiscalCode.Length == 16;
            else if (id == DefinedCompanyTypes.Other.Id)
                return fiscalCode.Length == 11;

            return true; // For other company types no restrictions
        }
    }

    public class UpdateBorrowerCommandValidator : AbstractValidator<UpdateBorrowerCommand> {
        public UpdateBorrowerCommandValidator() {
            RuleFor(x => x.BorrowerId)
                .NotEmpty().WithMessage("EmptyBorrowerId");

            RuleFor(x => x.CompanyName)
                    .NotEmpty().WithMessage("EmptyCompanyName");

            RuleFor(x => x.CompanyTypeId)
                .NotEmpty().WithMessage("EmptyCompanyType");

            RuleFor(x => x.VATNumber)
                .NotEmpty().WithMessage("EmptyVATNumber")
                .Length(11).WithMessage("VATNumberLengthRestriction");

            RuleFor(x => x.FiscalCode)
                .NotEmpty().WithMessage("EmptyFiscalCode");
        }
    }
}
