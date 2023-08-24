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

    public class ResetPasswordCommand : IRequest<bool> {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool> {

        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IMailBodyService _mailBodyService;
        private readonly IUserVerificationAndResetRepository _userVerificationAndResetRepository;
        private readonly IHasherService _hasherService;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ResetPasswordCommandHandler> _logger;

        public ResetPasswordCommandHandler(IUserRepository userRepository,
                                           IMailService mailService,
                                           IMailBodyService mailBodyService,
                                           IUserVerificationAndResetRepository userVerificationAndResetRepository,
                                           IHasherService hasherService,
                                           IStringLocalizer<LocalizationResources> localizer,
                                           IUnitOfWork unitOfWork,
                                           ILogger<ResetPasswordCommandHandler> logger) {
            _userRepository = userRepository;
            _mailService = mailService;
            _mailBodyService = mailBodyService;
            _userVerificationAndResetRepository = userVerificationAndResetRepository;
            _hasherService = hasherService;
            _localization = localizer;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken) {

            try {
                // we know for sure row exists, because it was created when first registered
                var entity = await _userVerificationAndResetRepository.GetByEmailAsync(request.Email);

                var passwordToken = entity.PasswordResetToken;
                var passwordTokenExpiry = entity.PasswordResetTokenExpiry;

                if (passwordToken is null)
                    throw new ForbiddenException(_localization.GetString("InvalidToken").Value);

                if (passwordTokenExpiry is null)
                    throw new ForbiddenException(_localization.GetString("InvalidToken").Value);

                if (passwordToken == request.Token
                    && passwordTokenExpiry > DateTime.Now) {
                    var tuple = _hasherService.HashPassword(request.Password);
                    var passwordHash = tuple.Item1;
                    var passwordSalt = tuple.Item2;

                    await _userRepository.ChangePasswordAsync(request.Email, passwordHash, passwordSalt);

                    var body = await _mailBodyService.GetSuccessfulPasswordChangeMailBodyAsync();
                    var subject = "Password Reset";
                    var mailData = new MailData(request.Email, subject, body);
                    var flag = await _mailService.SendAsync(mailData, cancellationToken);
                    if (flag is false)
                        throw new ThirdPartyException(_localization.GetString("SendEmailError").Value);

                    var dbFlag = await _unitOfWork.SaveChangesAsync();
                    if (dbFlag is false)
                        throw new DatabaseException(_localization.GetString("DatabaseException").Value);
                    return true;
                }
                else if (passwordToken == request.Token
                    && passwordTokenExpiry < DateTime.Now)
                    throw new UnauthorizedException(_localization.GetString("TokenExpired").Value);
                else
                    throw new ForbiddenException(_localization.GetString("InvalidToken").Value);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Reset Password Command Handler", request);

                throw;
            }
        }
    }

    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand> {
        public ResetPasswordCommandValidator() {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("EmptyEmail")
                .EmailAddress().WithMessage("EmailFormatRestriction");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("EmptyToken");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("EmptyPassword")
                .MinimumLength(8).WithMessage("PasswordMinimumLengthRestriction")
                .MaximumLength(16).WithMessage("PasswordMaximumLengthRestriction")
                .Matches(@"[A-Z]+").WithMessage("PasswordUppercaseLetterRestriction")
                .Matches(@"[a-z]+").WithMessage("PasswordLowercaseLetterRestriction")
                .Matches(@"[0-9]+").WithMessage("PasswordNumberRestriction");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("PasswordsDontMatch");
        }
    }
}
