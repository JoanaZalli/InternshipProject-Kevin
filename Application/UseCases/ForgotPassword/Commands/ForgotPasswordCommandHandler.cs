using Application.Exceptions.ClientErrors;
using Application.Exceptions.ServerErrors;
using Application.Interfaces.Authentication;
using Application.Interfaces.Email;
using Application.Persistance;
using Application.Persistance.Common;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ForgotPassword.Commands {

    public class ForgotPasswordCommand : IRequest<bool> {
        public string Email { get; set; } = null!;
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool> {

        private readonly IMailBodyService _mailBodyService;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly ITokenService _recoveryTokenService;
        private readonly IUserVerificationAndResetRepository _userVerificationAndResetRepository;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ForgotPasswordCommandHandler> _logger;


        public ForgotPasswordCommandHandler(IMailBodyService mailBodyService,
                                            IUserRepository userRepository,
                                            IMailService mailService,
                                            ITokenService recoveryTokenService,
                                            IUserVerificationAndResetRepository userVerificationAndResetRepository,
                                            IStringLocalizer<LocalizationResources> localizer,
                                            IUnitOfWork unitOfWork,
                                            ILogger<ForgotPasswordCommandHandler> logger) {
            _mailBodyService = mailBodyService;
            _userRepository = userRepository;
            _mailService = mailService;
            _recoveryTokenService = recoveryTokenService;
            _userVerificationAndResetRepository = userVerificationAndResetRepository;
            _localization = localizer;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken) {

            try {
                if (await _userRepository.ContainsEmailAsync(request.Email) is false)
                    throw new NotFoundException(_localization.GetString("EmailDoesntExist").Value);

                // generate & set the token in the db. This is the token that is just for pass reset
                var token = await _recoveryTokenService.GeneratePasswordTokenAsync();
                await _userVerificationAndResetRepository.SetPasswordTokenAsync(request.Email, token, DateTime.Now.AddMinutes(15));

                var body = await _mailBodyService.GetForgotPasswordMailBodyAsync(request.Email, token);
                string subject = "Reset Password Request";
                var mailData = new MailData(request.Email, subject, body);
                var flag = await _mailService.SendAsync(mailData, cancellationToken);
                if (flag is false)
                    throw new ThirdPartyException(_localization.GetString("SendEmailError").Value);

                var dbFlag = await _unitOfWork.SaveChangesAsync();
                if (dbFlag is false)
                    throw new DatabaseException(_localization.GetString("DatabaseException").Value);

                return true;
            }
            catch (Exception ex) {
                _logger.LogError("Error in Forgot Password Command Handler", request);

                throw;
            }
        }
    }

    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand> {
        public ForgotPasswordCommandValidator() {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("EmptyEmail")
                .EmailAddress().WithMessage("EmailFormatRestriction");
        }
    }
}
