using Application.Exceptions.ClientErrors;
using Application.Persistance;
using Infrastructure.Services.Common;
using InternshipProject.Localizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Services.Authentication.PermissionPolicyConfigurations {
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement> {

        private readonly IUserRepository _userRepository;
        private readonly IStringLocalizer<LocalizationResources> _localizer;

        public PermissionAuthorizationHandler(IUserRepository userRepository, IStringLocalizer<LocalizationResources> localizer) {
            _userRepository = userRepository;
            _localizer = localizer;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement) {

            // 1. Check if user is logged in
            string? userId = context.User.Claims
                               .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (!Guid.TryParse(userId, out Guid parsedUserId))
                throw new UnauthorizedException(_localizer.GetString("UnathorizedAccess").Value);

            // 2. Check if user's session is expired
            string? dateTimeString = context.User.Claims
                               .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?.Value;

            _ = long.TryParse(dateTimeString, out var exp);

            var dateTime = DateTimeOffset.FromUnixTimeSeconds(exp);

            if (dateTime <= DateTime.Now)
                throw new UnauthorizedException(_localizer.GetString("LoginExpired").Value);

            // 3. Check his permissions
            var permissions = await _userRepository.GetPermissionsAsync(parsedUserId);

            var requestPermissions = requirement.Permission.Split(',').ToList();

            foreach (var requestPermission in requestPermissions)
                if (permissions.Contains(requestPermission.Trim())) {
                    context.Succeed(requirement); // a) has the permissions
                    return;
                }
            throw new ForbiddenException(_localizer
                .GetString("MissingPermission").Value); // b) doesn't have the permissions
        }
    }
}
