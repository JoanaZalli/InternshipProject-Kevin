using Application.Exceptions.ClientErrors;
using Application.UseCases.LoanJourney.Commands;
using IntegrationTest.Configurations;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTest.LoanTests {
    public class CreateLoanTests : BaseIntegrationTest {
        public CreateLoanTests(IntegrationTestWebFactory factory) : base(factory) {
        }

        [Fact]
        public async void HandleCreateLoan_WrongApplication_ThrowsNotFoundException() {

            // Arrange
            var command = new CreateLoanCommand {
                ApplicationId = Guid.Parse("23EADAC2-3BBB-421B-9A5B-AFF07EB74C41"),
                LenderId = Guid.Parse("0F3C377F-89AD-4FD6-AF55-62F783B0EA52")
            };

            // Act && Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await _mediator.Send(command));
        }

        [Fact]
        public async void HandleCreateLoan_WrongLender_ThrowsNotFoundException() {

            // Arrange
            var command = new CreateLoanCommand {
                ApplicationId = Guid.Parse("9038FF25-5A61-46D0-BC8F-B49F23E42280"),
                LenderId = Guid.Parse("23EADAC2-3BBB-421B-9A5B-AFF07EB74C41")
            };

            // Act && Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await _mediator.Send(command));
        }

        [Fact]
        public async void HandleCreateLoan_AlreadyApprovedApplication_ThrowsConflictException() {

            // Arrange
            var command = new CreateLoanCommand {
                ApplicationId = Guid.Parse("5EFCFE9C-4759-4935-9D59-EFE9160C9C79"),
                LenderId = Guid.Parse("0F3C377F-89AD-4FD6-AF55-62F783B0EA52")
            };

            // Act && Assert
            await Assert.ThrowsAsync<ConflictException>(async () => await _mediator.Send(command));
        }

        [Fact]
        public async void HandleCreateLoan_UneligibleLender_ThrowsConflictException() {

            // Arrange
            var command = new CreateLoanCommand {
                ApplicationId = Guid.Parse("6cc70007-2aea-45cc-af05-56968b5abc13"),
                LenderId = Guid.Parse("8D1AC5ED-0E1E-4DE1-A7FC-A7DF9E095653") // ineligible due to company type
            };

            // Act && Assert
            await Assert.ThrowsAsync<InvalidRequestException>(async () => await _mediator.Send(command));
        }

        [Fact]
        public async void HandleCreateLoan_HappyPath_CreatesLoan() {

            // Arrange
            var command = new CreateLoanCommand {
                ApplicationId = Guid.Parse("6cc70007-2aea-45cc-af05-56968b5abc13"),
                LenderId = Guid.Parse("0F3C377F-89AD-4FD6-AF55-62F783B0EA52")
            };

            // Act
            var result = await _mediator.Send(command);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id.ToString());
            Assert.NotNull(result.InterestRate.ToString());
            Assert.NotNull(result.RequestedAmount.ToString());
            Assert.NotNull(result.ReferenceRate.ToString());
            Assert.NotNull(result.Status.ToString());
            Assert.NotNull(result.Tenor.ToString());

            var loan = await _dbContext.Loans.FirstOrDefaultAsync(x => x.Id == result.Id);
            Assert.NotNull(loan);
            Assert.Equal(command.ApplicationId, loan.ApplicationId);

            _ = _dbContext.Remove(loan);
            await _dbContext.SaveChangesAsync();
        }
    }
}
