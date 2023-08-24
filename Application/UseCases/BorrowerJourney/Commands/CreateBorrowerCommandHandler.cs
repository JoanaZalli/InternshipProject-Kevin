using Application.Exceptions.ClientErrors;
using Application.Exceptions.ServerErrors;
using Application.Interfaces.Authentication;
using Application.Persistance;
using Application.Persistance.Common;
using Application.UseCases.BorrowerJourney.Results;
using Application.UseCases.Common;
using AutoMapper;
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

    public class CreateBorrowerCommmand : IRequest<BorrowerCommandResult> {
        public string AccessToken { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string CompanyTypeId { get; set; } = null!;
        public string VATNumber { get; set; } = null!;
        public string FiscalCode { get; set; } = null!;
    }

    public class CreateBorrowerCommandHandler : IRequestHandler<CreateBorrowerCommmand, BorrowerCommandResult> {

        private readonly IUserRepository _userRepository;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly IMapper _mapper;
        private readonly ICompanyTypeRepository _companyTypeRepository;
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly IJwtToken _jwtToken;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _roleRepository;
        private readonly FinHubConnectionSettings _finHubConnectionSettings;
        private readonly ILogger<CreateBorrowerCommandHandler> _logger;


        public CreateBorrowerCommandHandler(IStringLocalizer<LocalizationResources> localization,
                                            IUserRepository userRepository,
                                            IMapper mapper,
                                            ICompanyTypeRepository companyTypeRepository,
                                            IBorrowerRepository borrowerRepository,
                                            IJwtToken jwtToken,
                                            IUnitOfWork unitOfWork,
                                            IRoleRepository roleRepository,
                                            IOptions<FinHubConnectionSettings> finHubConnectionSettings,
                                            ILogger<CreateBorrowerCommandHandler> logger) {
            _localization = localization;
            _userRepository = userRepository;
            _mapper = mapper;
            _companyTypeRepository = companyTypeRepository;
            _borrowerRepository = borrowerRepository;
            _jwtToken = jwtToken;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
            _finHubConnectionSettings = finHubConnectionSettings.Value;
            _logger = logger;
        }

        public async Task<BorrowerCommandResult> Handle(CreateBorrowerCommmand request, CancellationToken cancellationToken) {

            try {
                var userId = _jwtToken.GetUserId(request.AccessToken);

                if (await _userRepository.ContainsAsync(userId) is false)
                    throw new UnauthorizedException(_localization.GetString("UnathorizedAccess").Value);

                var companyTypeId = Guid.Parse(request.CompanyTypeId);

                if (await _companyTypeRepository.ContainsAsync(companyTypeId) is false)
                    throw new NotFoundException(_localization.GetString("CompanyTypeDoesntExist").Value);

                // fiscal codes must be unique for user
                if (await _borrowerRepository.IsFiscalCodeUniqueAsync(userId, request.FiscalCode) is false)
                    throw new ConflictException(_localization.GetString("DuplicateFiscalCode").Value);

                // validate length
                if (IsValid(companyTypeId, request.FiscalCode) is false)
                    throw new InvalidRequestException(_localization.GetString("FiscalCodeLengthRestriction").Value);

                var borrower = _mapper.Map<Borrower>(request);
                var borrowerId = Guid.NewGuid();
                borrower.Id = borrowerId;
                borrower.UserId = userId;
                borrower.CompanyType = await _companyTypeRepository.GetByIdAsync(companyTypeId);
                borrower.CompanyProfile = await GetCompanyProfile(request.CompanyName, _finHubConnectionSettings.AuthToken);

                await _borrowerRepository.CreateAsync(borrower);

                // automatically add role borrower -> Can see his own borrow, add application etc.
                await _userRepository.AddRoleAsync(userId,
                                                   await _roleRepository.GetByIdAsync(DefinedRoles.Borrower.Id));

                var flag = await _unitOfWork.SaveChangesAsync();
                if (flag is false)
                    throw new DatabaseException(_localization.GetString("DatabaseException").Value);

                return new BorrowerCommandResult {
                    Id = borrowerId,
                    CompanyName = borrower.CompanyName,
                    CompanyType = borrower.CompanyType.Type
                };

            }
            catch (Exception ex) {
                _logger.LogError("Error in Create Borrower Command Handler", request);

                throw;
            }
        }

        // should be private, public just for test
        public static async Task<CompanyProfile> GetCompanyProfile(string companyName, string authToken) {

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
                throw new ThirdPartyException(response.ReasonPhrase);

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CompanyProfile>(stringResponse,
                                                              new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            result.Id = Guid.NewGuid();
            return result is null ? throw new ThirdPartyException("Invalid JSON data.") : result;
        }

        // should be private, public just for test
        public static bool IsValid(Guid id, string fiscalCode) {
            if (id == DefinedCompanyTypes.SoleProprietorship.Id)
                return fiscalCode.Length == DefinedCompanyTypes.SoleProprietorship.FiscalCodeLength;
            else if (id == DefinedCompanyTypes.Other.Id)
                return fiscalCode.Length == DefinedCompanyTypes.Other.FiscalCodeLength;

            return true; // For other company types no restrictions
        }
    }

    public class CreateBorrowerCommandValidator : AbstractValidator<CreateBorrowerCommmand> {
        public CreateBorrowerCommandValidator() {
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
