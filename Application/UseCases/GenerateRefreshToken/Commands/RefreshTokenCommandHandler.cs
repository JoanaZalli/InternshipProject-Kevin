using Application.Exceptions.ClientErrors;
using Application.Exceptions.ServerErrors;
using Application.Interfaces.Authentication;
using Application.Persistance;
using Application.Persistance.Common;
using Application.UseCases.GenerateRefreshToken.Results;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.GenerateRefreshToken.Commands {

    public class RefreshTokenCommand : IRequest<RefreshTokenResult> {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResult> {

        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IUserVerificationAndResetRepository _userVerificationAndResetRepository;
        private readonly IJwtToken _jwtToken;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;

        public RefreshTokenCommandHandler(ITokenService tokenService, IUserRepository userRepository, IUserVerificationAndResetRepository userVerificationAndResetRepository, IJwtToken jwtToken, IStringLocalizer<LocalizationResources> localizer, IUnitOfWork unitOfWork, ILogger<RefreshTokenCommandHandler> logger) {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _userVerificationAndResetRepository = userVerificationAndResetRepository;
            _jwtToken = jwtToken;
            _localization = localizer;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken) {

            try {
                var userId = _jwtToken.GetUserId(request.AccessToken);

                if (await _userRepository.ContainsAsync(userId) is false)
                    throw new NotFoundException(_localization.GetString("InvalidToken").Value);

                var user = await _userRepository.GetByIdAsync(userId);

                var entity = await _userVerificationAndResetRepository.GetByEmailAsync(user.Email);

                var oldRefreshToken = entity.RefreshToken;
                var oldRefreshTokenExpiry = entity.RefreshTokenExpiry;

                if (oldRefreshToken is null || oldRefreshTokenExpiry is null)
                    throw new UnauthorizedException(_localization.GetString("InvalidToken").Value);
                else if (request.RefreshToken != oldRefreshToken)
                    throw new UnauthorizedException(_localization.GetString("InvalidToken").Value);
                else if (oldRefreshTokenExpiry <= DateTime.Now)
                    throw new UnauthorizedException(_localization.GetString("TokenExpired").Value);

                var refreshToken = await _tokenService.GenerateRefreshTokenAsync();
                await _userRepository.SetRefreshTokenAsync(user.Id, refreshToken, DateTime.Now.AddDays(7));

                var roles = _jwtToken.GetRoles(request.AccessToken);
                var accessToken = _jwtToken.GenerateToken(userId, user.Username, roles.AsEnumerable()) ?? throw new ServerException();

                var flag = await _unitOfWork.SaveChangesAsync();
                if (flag is false)
                    throw new DatabaseException(_localization.GetString("DatabaseException").Value);

                return new RefreshTokenResult {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            catch (Exception ex) {
                _logger.LogError("Error in Refresh Token Command Handler", request);

                throw;
            }
        }
    }

    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand> {
        public RefreshTokenCommandValidator() {
            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage("EmptyAccessToken");
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("EmptyRefreshToken");
        }
    }
}
