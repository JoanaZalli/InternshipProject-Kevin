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

    public class GetBorrowerQuery : IRequest<BorrowerQueryResult> {
        public Guid BorrowerId { get; set; }
    }

    public class GetBorrowerQueryHandler : IRequestHandler<GetBorrowerQuery, BorrowerQueryResult> {

        private readonly IBorrowerRepository _borrowerRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly ILogger<GetBorrowerQueryHandler> _logger;

        public GetBorrowerQueryHandler(IBorrowerRepository borrowerRepository,
                                       IMapper mapper,
                                       IStringLocalizer<LocalizationResources> localizer,
                                       ILogger<GetBorrowerQueryHandler> logger) {
            _borrowerRepository = borrowerRepository;
            _mapper = mapper;
            _localization = localizer;
            _logger = logger;
        }

        public async Task<BorrowerQueryResult> Handle(GetBorrowerQuery request, CancellationToken cancellationToken) {

            try {
                if (await _borrowerRepository.ContainsAsync(request.BorrowerId) is false)
                    throw new NotFoundException(_localization.GetString("BorrowerDoesntExist").Value);

                var borrower = await _borrowerRepository.GetByIdAsync(request.BorrowerId);

                var result = _mapper.Map<BorrowerQueryResult>(borrower);
                return result;
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get Borrower Query Handler", request);

                throw;
            }
        }
    }

    public class GetBorrowerQueryValidator : AbstractValidator<GetBorrowerQuery> {
        public GetBorrowerQueryValidator() {
            RuleFor(x => x.BorrowerId)
                .NotEmpty().WithMessage("EmptyBorrowerId");
        }
    }
}
