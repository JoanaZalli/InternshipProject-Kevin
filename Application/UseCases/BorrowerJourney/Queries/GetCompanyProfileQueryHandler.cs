using Application.Exceptions.ClientErrors;
using Application.Persistance;
using Application.UseCases.BorrowerJourney.Results;
using AutoMapper;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.BorrowerJourney.Queries {

    public class GetCompanyProfileQuery : IRequest<CompanyProfileResult> {
        public Guid BorrowerId { get; set; }
    }


    public class GetCompanyProfileQueryHandler : IRequestHandler<GetCompanyProfileQuery, CompanyProfileResult> {

        private readonly ICompanyProfileRepository _companyProfileRepository;
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly ILogger<GetCompanyProfileQueryHandler> _logger;


        public GetCompanyProfileQueryHandler(IMapper mapper, IBorrowerRepository borrowerRepository, ICompanyProfileRepository companyProfileRepository, IStringLocalizer<LocalizationResources> localization, ILogger<GetCompanyProfileQueryHandler> logger) {
            _mapper = mapper;
            _borrowerRepository = borrowerRepository;
            _companyProfileRepository = companyProfileRepository;
            _localization = localization;
            _logger = logger;
        }

        public async Task<CompanyProfileResult> Handle(GetCompanyProfileQuery request, CancellationToken cancellationToken) {

            try {
                if (await _borrowerRepository.ContainsAsync(request.BorrowerId) is false)
                    throw new NotFoundException(_localization.GetString("BorrowerDoesntExist").Value);

                var companyProfile = await _companyProfileRepository.GetByBorrowerAsync(request.BorrowerId);

                return _mapper.Map<CompanyProfileResult>(companyProfile);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get Company Profile Query Handler", request);

                throw;
            }
        }
    }

    public class GetCompanyProfileQueryValidator : AbstractValidator<GetCompanyProfileQuery> {
        public GetCompanyProfileQueryValidator() {
            RuleFor(x => x.BorrowerId)
                .NotEmpty().WithMessage("EmptyBorrowerId");
        }
    }

}
