using Application.Interfaces.Authentication;
using Infrastructure.Services.Authentication;
using Infrastructure.Services.Common;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UnitTests.Infrastructure {
    public class JwtTokenServiceTests {

        private readonly IJwtToken _jwtToken;

        public JwtTokenServiceTests() {
            var jwtSettings = new JwtSettings {
                Secret = "OKE8zlLQypY77kjIMb7FZNSOWkFhapUt0fsUaArLTQRvaMUXifmHghl8rO0WZd5g",
                Issuer = "InternshipProject",
                Audience = "InternshipProject",
                ExpiryDays = 1
            };
            var jwtOptions = Options.Create(jwtSettings);
            _jwtToken = new JwtToken(jwtOptions);
        }

        [Fact]
        public void GenerateToken_ReturnsValidJwtToken() {

            // Arrange
            var userId = Guid.NewGuid();
            var username = "testuser";
            var roles = new List<string> { "Role1", "Role2" };

            // Act
            var token = _jwtToken.GenerateToken(userId, username, roles);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var roleClaims = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);

            Assert.Equal(userId.ToString(), jwtToken.Subject);
            Assert.Equal(username, jwtToken.Claims.FirstOrDefault(c => c.Type == "username")?.Value);

            Assert.Equal(roles.Count, roleClaims.Count());
            roles.ForEach(x => Assert.Contains(x, roleClaims.Select(c => c.Value)));
        }

        [Fact]
        public void GenerateToken_EmptyRoles_ReturnsValidJwtToken() {

            // Arrange
            var userId = Guid.NewGuid();
            var username = "testuser";
            var roles = new List<string>();

            // Act
            var token = _jwtToken.GenerateToken(userId, username, roles);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var roleClaims = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);

            Assert.Equal(userId.ToString(), jwtToken.Subject);
            Assert.Equal(username, jwtToken.Claims.FirstOrDefault(c => c.Type == "username")?.Value);

            Assert.Empty(roleClaims);
        }

        [Fact]
        public void GenerateToken_NullId_ReturnsNull() {
            // Arrange
            Guid userId = Guid.Empty;
            var username = "testuser";
            var roles = new List<string> { "Role1" };

            // Act
            var result = _jwtToken.GenerateToken(userId, username, roles);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GenerateToken_NullUsername_ReturnsNull() {
            // Arrange
            Guid userId = Guid.NewGuid();
            string? username = null;
            var roles = new List<string> { "Role1" };

            // Act
            var result = _jwtToken.GenerateToken(userId, username, roles);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GenerateToken_EmptyUsername_ReturnsNull() {
            // Arrange
            Guid userId = Guid.NewGuid();
            string? username = "";
            var roles = new List<string> { "Role1" };

            // Act
            var result = _jwtToken.GenerateToken(userId, username, roles);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetRoles_ReturnsActualRoles() {
            // Arrange
            Guid userId = Guid.NewGuid();
            string? username = "testuser";
            var roles = new List<string> { "Role1", "Role2" };
            var token = _jwtToken.GenerateToken(userId, username, roles);

            // Act
            var result = _jwtToken.GetRoles(token);

            // Assert
            Assert.True(result.SequenceEqual(roles));
        }

        [Fact]
        public void GetRoles_EmptyRoles_ReturnEmptyList() {
            // Arrange
            Guid userId = Guid.NewGuid();
            string? username = "testuser";
            var roles = new List<string> { };
            var token = _jwtToken.GenerateToken(userId, username, roles);

            // Act
            var result = _jwtToken.GetRoles(token);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void IsExpired_TokenIsStillValid_ReturnsFalse() {
            // Arrange
            Guid userId = Guid.NewGuid();
            string? username = "testuser";
            var roles = new List<string> { };
            var token = _jwtToken.GenerateToken(userId, username, roles);

            // Act
            var result = _jwtToken.IsExpired(token);

            // Assert
            Assert.False(result);
        }
    }
}

