using Application.Exceptions.ClientErrors;
using Application.Exceptions.ServerErrors;
using Application.Interfaces.Email;
using Application.Persistance;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ForgotUsername.Queries {

    public class ForgotUsernameQuery : IRequest<bool> {
        public string Email { get; set; } = null!;
    }

    public class ForgotUsernameQueryHandler : IRequestHandler<ForgotUsernameQuery, bool> {

        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IMailBodyService _mailBodyService;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly ILogger<ForgotUsernameQueryHandler> _logger;


        public ForgotUsernameQueryHandler(IUserRepository userRepository,
                                          IMailService mailService,
                                          IMailBodyService mailBodyService,
                                          IStringLocalizer<LocalizationResources> localizer,
                                          ILogger<ForgotUsernameQueryHandler> logger) {
            _userRepository = userRepository;
            _mailService = mailService;
            _mailBodyService = mailBodyService;
            _localization = localizer;
            _logger = logger;
        }

        public async Task<bool> Handle(ForgotUsernameQuery request, CancellationToken cancellationToken) {

            try {
                if (await _userRepository.ContainsEmailAsync(request.Email) is false)
                    throw new NotFoundException(_localization.GetString("EmailDoesntExist").Value);

                var user = await _userRepository.GetByEmailAsync(request.Email);

                var body = await _mailBodyService.GetForgotUsernameMailBodyAsync(user.Username);
                string subject = "Forgot Username";
                var mailData = new MailData(request.Email, subject, body);

                var flag = await _mailService.SendAsync(mailData, cancellationToken);
                if (flag is false)
                    throw new ThirdPartyException(_localization.GetString("SendEmailError").Value);

                return true;
            }
            catch (Exception ex) {
                _logger.LogError("Error in Forgot Username Query Handler", request);

                throw;
            }
        }
    }

    public class ForgotUsernameQueryValidator : AbstractValidator<ForgotUsernameQuery> {
        public ForgotUsernameQueryValidator() {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("EmptyEmail")
                .EmailAddress().WithMessage("EmailFormatRestriction");
        }
    }
}
