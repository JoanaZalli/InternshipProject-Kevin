using Application.Interfaces.Authentication;
using Infrastructure.Services.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services.Authentication {
    public class JwtToken : IJwtToken {

        private readonly JwtSettings _jwtSettings;

        public JwtToken(IOptions<JwtSettings> jwtOptions) {
            _jwtSettings = jwtOptions.Value;
        }

        public string GenerateToken(Guid userId, string username, IEnumerable<string> roles) {

            if (string.IsNullOrEmpty(userId.ToString()) || userId == Guid.Empty)
                return null!;

            if (string.IsNullOrEmpty(username))
                return null!;

            // add the token claims
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim("username", username)
            };

            foreach (var role in roles) {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                                         SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.Now.AddDays(_jwtSettings.ExpiryDays),
                claims: claims,
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public IEnumerable<string> GetRoles(string accessToken) {
            var token = new JwtSecurityToken(accessToken);
            var roles = token.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
            return roles;
        }

        public Guid GetUserId(string accessToken) {
            var token = new JwtSecurityToken(accessToken);
            var userId = Guid.Parse(token.Claims.First(x => x.Type == "sub").Value);
            return userId;
        }

        public bool IsExpired(string accessToken) {
            var token = new JwtSecurityToken(accessToken);

            if (token.ValidTo <= DateTime.Now)
                return true;
            return false;
        }
    }
}
