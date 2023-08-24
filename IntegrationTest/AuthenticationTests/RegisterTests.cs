using Application.Exceptions.ClientErrors;
using Application.UseCases.Authentication.Commands;
using IntegrationTest.Configurations;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTest.AuthenticationTests {

    public class RegisterTests : BaseIntegrationTest {

        public RegisterTests(IntegrationTestWebFactory factory) : base(factory) {
        }

        [Fact]
        public async void HandleRegister_DuplicateEmail_ThrowsConflictException() {

            // Arrange
            var command = new RegisterCommand {
                FirstName = "Test",
                LastName = "Test",
                Username = "finalfinal3",
                Email = "finalfinal10@ffinal.com",
                Password = "Testtest123",
                Phone = "787878788",
                Prefix = "+55"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ConflictException>(async () => await _mediator.Send(command));
        }

        [Fact]
        public async void HandleRegister_DuplicateUsername_ThrowsConflictException() {

            // Arrange
            var command = new RegisterCommand {
                FirstName = "Test",
                LastName = "Test",
                Username = "finalfinal10",
                Email = "final@royalka.com",
                Password = "Testtest123",
                Phone = "787878788",
                Prefix = "+55"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ConflictException>(async () => await _mediator.Send(command));
        }

        [Fact]
        public async void HandleRegister_HappyPath_UserCreated_TokensCreated() {
            // Arrange
            var command = new RegisterCommand {
                FirstName = "Test",
                LastName = "Test",
                Username = "usernameTest1",
                Email = "usernameTest1@agoaeop.com",
                Password = "Testtest123",
                Phone = "787878788",
                Prefix = "+55"
            };

            // Act
            var result = await _mediator.Send(command);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id.ToString());
            Assert.NotNull(result.Username);
            Assert.NotNull(result.Email);

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == result.Id);
            Assert.NotNull(user);
            var tokens = await _dbContext.UserVerificationAndReset.FirstOrDefaultAsync(x => x.UserEmail
                                                                                            == result.Email);
            Assert.NotNull(tokens);

            _ = _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
