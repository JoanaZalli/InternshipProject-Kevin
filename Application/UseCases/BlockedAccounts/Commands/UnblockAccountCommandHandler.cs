using Application.Exceptions.ClientErrors;
using Application.Exceptions.ServerErrors;
using Application.Persistance;
using Application.Persistance.Common;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.UnblockAccount.Command {

    public class UnblockAccountCommand : IRequest<bool> {
        public Guid UserId { get; set; }
    }

    public class UnblockAccountCommandHandler : IRequestHandler<UnblockAccountCommand, bool> {

        private readonly IUserRepository _userRepository;
        private readonly IStringLocalizer<LocalizationResources> _localizer;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UnblockAccountCommandHandler> _logger;


        public UnblockAccountCommandHandler(IUserRepository userRepository,
                                            IStringLocalizer<LocalizationResources> localizer,
                                            IUnitOfWork unitOfWork,
                                            ILogger<UnblockAccountCommandHandler> logger) {
            _userRepository = userRepository;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(UnblockAccountCommand request, CancellationToken cancellationToken) {

            try {
                if (await _userRepository.ContainsAsync(request.UserId) is false)
                    throw new NotFoundException(_localizer.GetString("UsernameDoesntExist").Value);

                await _userRepository.ResetTriesAsync(request.UserId);
                await _userRepository.UnblockAccountAsync(request.UserId);

                var flag = await _unitOfWork.SaveChangesAsync();
                if (flag is false)
                    throw new DatabaseException(_localizer.GetString("DatabaseException").Value);

                return true;
            }
            catch (Exception ex) {
                _logger.LogError("Error in Unblock Account Command Handler", request);

                throw;
            }
        }
    }

    public class UnblockAccountCommandValidator : AbstractValidator<UnblockAccountCommand> {
        public UnblockAccountCommandValidator() {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("EmptyUserId");
        }
    }
}
