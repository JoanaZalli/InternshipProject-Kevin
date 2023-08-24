using Application.Exceptions.ClientErrors;
using Application.Exceptions.ServerErrors;
using Application.Persistance;
using Application.Persistance.Common;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.LenderMatrixCases.Commands {

    public class DeleteLenderMatrixCommand : IRequest<bool> {
        public Guid ProductId { get; set; }
        public Guid LenderId { get; set; }
    }

    public class DeleteLenderMatrixCommandHandler : IRequestHandler<DeleteLenderMatrixCommand, bool> {

        private readonly ILenderMatrixRepository _lenderMatrixRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly ILogger<DeleteLenderMatrixCommandHandler> _logger;

        public DeleteLenderMatrixCommandHandler(ILenderMatrixRepository lenderMatrixRepository,
                                                IUnitOfWork unitOfWork,
                                                IStringLocalizer<LocalizationResources> localization,
                                                ILogger<DeleteLenderMatrixCommandHandler> logger) {
            _lenderMatrixRepository = lenderMatrixRepository;
            _unitOfWork = unitOfWork;
            _localization = localization;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteLenderMatrixCommand request, CancellationToken cancellationToken) {

            try {
                if (await _lenderMatrixRepository.ContainsAsync(request.LenderId, request.ProductId) is false)
                    throw new NotFoundException(_localization.GetString("MatrixDoesntExist").Value);

                await _lenderMatrixRepository.DeleteAsync(request.ProductId, request.LenderId);

                var flag = await _unitOfWork.SaveChangesAsync();
                if (flag is false)
                    throw new DatabaseException(_localization.GetString("DatabaseException"));

                return true;
            }
            catch (Exception ex) {
                _logger.LogError("Error in Delete Lender Matrix Command Handler", request);

                throw;
            }
        }

        public class DeleteLenderMatrixCommandValidator : AbstractValidator<DeleteLenderMatrixCommand> {
            public DeleteLenderMatrixCommandValidator() {
                RuleFor(x => x.ProductId)
                    .NotEmpty().WithMessage("EmptyProductId");

                RuleFor(x => x.LenderId)
                    .NotEmpty().WithMessage("EmptyLenderId");
            }
        }
    }
}
