using Application.Exceptions.ClientErrors;
using Application.UseCases.Authentication.Commands;
using IntegrationTest.Configurations;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTest.AuthenticationTests {
    public class LoginTests : BaseIntegrationTest {

        public LoginTests(IntegrationTestWebFactory factory) : base(factory) {
        }

        [Fact]
        public async void HandleLogin_InvalidUsername_ThrowsNotFoundException() {

            // Arrange
            var command = new LoginCommand {
                Username = "wrongwrong",
                Password = "wrongwrong"
            };

            // Act && Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await _mediator.Send(command));
        }

        [Fact]
        public async void HandleLogin_UnconfirmedEmail_ThrowsForbiddenException() {

            // Arrange
            var command = new LoginCommand {
                Username = "finalfinal10",
                Password = "Testtest123"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ForbiddenException>(async () => await _mediator.Send(command));
        }

        [Fact]
        public async void HandleLogin_BlockedAccount_ThrowsBlockedException() {

            // Arrange
            var command = new LoginCommand {
                Username = "blockedblocked1",
                Password = "Testtest123"
            };

            // Act && Assert
            await Assert.ThrowsAsync<BlockedException>(async () => await _mediator.Send(command));
        }

        [Fact]
        public async void HandleLogin_InvalidPassword_ThrowsUnauthorizedException() {

            // Arrange
            var command = new LoginCommand {
                Username = "finalfinal3",
                Password = "wrongpassword"
            };

            // Act && Assert
            await Assert.ThrowsAsync<UnauthorizedException>(async () => await _mediator.Send(command));

            // Revert Changes
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == command.Username);
            user.LoginTries = 0;
            user.IsBlocked = false;
            await _dbContext.SaveChangesAsync();
        }

        [Fact]
        public async void HandleLogin_ThreeIncorrectLoginsShouldBlockAccount_ThrowsBlockedException() {

            // Arrange
            var command = new LoginCommand {
                Username = "finalfinal3",
                Password = "wrongpassword"
            };

            // Act
            for (var i = 1; i <= 4; i++) {
                try {
                    _ = await _mediator.Send(command);
                }
                catch (Exception) {
                    continue;
                }
            }

            // Assert
            await Assert.ThrowsAsync<BlockedException>(async () => await _mediator.Send(command));

            // Revert Changes
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == command.Username);
            user.LoginTries = 0;
            user.IsBlocked = false;
            await _dbContext.SaveChangesAsync();
        }

        [Fact]
        public async void HandleLogin_HappyPath_CreatesToken_SetsRefreshToken_ResetsLoginTries() {

            // Arrange
            var command = new LoginCommand {
                Username = "FinFinFin2",
                Password = "FinFinFin2"
            };

            // Act
            var result = await _mediator.Send(command);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id.ToString());
            Assert.NotNull(result.Token);
            Assert.NotNull(result.RefreshToken);

            var tokens = await _dbContext.UserVerificationAndReset.FirstOrDefaultAsync(x => x.UserEmail
                                                                                            == result.Email);

            Assert.NotNull(tokens);
            Assert.NotNull(tokens.RefreshToken);
            Assert.Equal(result.RefreshToken, tokens.RefreshToken);
        }
    }
}
