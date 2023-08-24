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

namespace Application.UseCases.ResendEmailVerification.Commands {

    public class ResendEmailVerificationCommand : IRequest<bool> {
        public string Email { get; set; } = null!;
    }

    public class ResendEmailVerificationCommandHandler : IRequestHandler<ResendEmailVerificationCommand, bool> {

        private readonly IMailBodyService _mailBodyService;
        private readonly IMailService _mailService;
        private readonly IUserVerificationAndResetRepository _userVerificationAndResetRepository;
        private readonly ITokenService _recoveryTokenService;
        private readonly IStringLocalizer<LocalizationResources> _localizer;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ResendEmailVerificationCommandHandler> _logger;

        public ResendEmailVerificationCommandHandler(IMailBodyService mailBodyService,
                                                     IMailService mailService,
                                                     IUserVerificationAndResetRepository userVerificationAndResetRepository,
                                                     ITokenService recoveryTokenService,
                                                     IStringLocalizer<LocalizationResources> localizer,
                                                     IUnitOfWork unitOfWork,
                                                     ILogger<ResendEmailVerificationCommandHandler> logger) {
            _mailBodyService = mailBodyService;
            _mailService = mailService;
            _userVerificationAndResetRepository = userVerificationAndResetRepository;
            _recoveryTokenService = recoveryTokenService;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(ResendEmailVerificationCommand request, CancellationToken cancellationToken) {

            try {
                var flag = await _userVerificationAndResetRepository.ContainsEmailAsync(request.Email);

                if (flag is false)
                    throw new NotFoundException(_localizer.GetString("EmailDoesntExist").Value);

                var token = await _recoveryTokenService.GenerateVerificationTokenAsync();
                var tokenExpiry = DateTime.Now.AddMinutes(30);

                var body = await _mailBodyService.GetVerificationMailBodyAsync(request.Email, token);

                await _userVerificationAndResetRepository.UpdateVerificationTokenAsync(request.Email, token, tokenExpiry);

                var subject = "Verify Your Email";
                var mailData = new MailData(request.Email, subject, body);

                var emailFlag = await _mailService.SendAsync(mailData, cancellationToken);
                if (emailFlag is false)
                    throw new ThirdPartyException(_localizer.GetString("SendEmailError").Value);

                var dbFlag = await _unitOfWork.SaveChangesAsync();
                if (dbFlag is false)
                    throw new DatabaseException(_localizer.GetString("DatabaseException").Value);

                return true;
            }
            catch (Exception ex) {
                _logger.LogError("Error in Resend Email Verification Command Handler", request);

                throw;
            }
        }
    }

    public class ResendEmailVerificationCommandValidator : AbstractValidator<ResendEmailVerificationCommand> {
        public ResendEmailVerificationCommandValidator() {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("EmptyEmail")
                .EmailAddress().WithMessage("EmailFormatRestriction");
        }
    }
}
