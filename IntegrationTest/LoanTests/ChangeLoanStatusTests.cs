using Application.Exceptions.ClientErrors;
using Application.UseCases.LoanJourney.Commands;
using Domain.Seeds;
using IntegrationTest.Configurations;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTest.LoanTests {
    public class ChangeLoanStatusTests : BaseIntegrationTest {

        public ChangeLoanStatusTests(IntegrationTestWebFactory factory) : base(factory) {
        }

        [Fact]
        public async void HandleLoanStatusChange_WrongLoan_ThrowsNotFoundException() {

            // Arrange
            var command = new ChangeLoanStatusCommand {
                LoanId = Guid.Parse("2DC1C106-6C3D-4AF4-98E3-3AF497A097F1"),
                StatusId = Guid.Parse("45E0D7E5-0C30-4EE0-B0C6-23379A9BD138")
            };

            // Act && Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await _mediator.Send(command));
        }

        [Fact]
        public async void HandleLoanStatusChange_WrongLoanStatus_ThrowsNotFoundException() {

            // Arrange
            var command = new ChangeLoanStatusCommand {
                LoanId = Guid.Parse("8CA089D6-0F2D-4EA9-A437-57B734510B08"),
                StatusId = Guid.Parse("D489F989-2747-489E-9EE7-10C8086391FC")
            };

            // Act && Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await _mediator.Send(command));
        }

        [Theory]
        [InlineData(DefinedLoanStatuses.Created.Name)]
        [InlineData(DefinedLoanStatuses.Erased.Name)]
        [InlineData(DefinedLoanStatuses.Defaulted.Name)]
        [InlineData(DefinedLoanStatuses.Disbursed.Name)]
        [InlineData(DefinedLoanStatuses.Guaranted.Name)]
        [InlineData(DefinedLoanStatuses.Rejected.Name)]
        [InlineData(DefinedLoanStatuses.Repaid.Name)]
        public async void HandleLoanStatusChange_HappyPath_ChangesApplicationStatusAccordingly(string status) {

            var loanId = Guid.Parse("CB11BDDE-CF3B-4794-80D3-E274E14644E7");
            var loan = await _dbContext.Loans.Include(x => x.Application).FirstOrDefaultAsync(x => x.Id == loanId);
            var application = loan.Application;

            // Arrange && Assert && Act
            switch (status) {

                case DefinedLoanStatuses.Created.Name:

                    var resultCreated = await _mediator.Send(new ChangeLoanStatusCommand {
                        LoanId = loanId,
                        StatusId = DefinedLoanStatuses.Created.Id
                    });
                    Assert.True(resultCreated);

                    Assert.Equal(DefinedLoanStatuses.Created.Id, loan.LoanStatusId);
                    Assert.Equal(DefinedApplicationStatuses.InCharge.Id, application.ApplicationStatusId);
                    break;

                case DefinedLoanStatuses.Erased.Name:

                    var resultErased = await _mediator.Send(new ChangeLoanStatusCommand {
                        LoanId = loanId,
                        StatusId = DefinedLoanStatuses.Erased.Id
                    });
                    Assert.True(resultErased);


                    Assert.Equal(DefinedLoanStatuses.Erased.Id, loan.LoanStatusId);
                    Assert.Equal(DefinedApplicationStatuses.Canceled.Id, application.ApplicationStatusId);
                    break;

                case DefinedLoanStatuses.Defaulted.Name:

                    var resultDefaulted = await _mediator.Send(new ChangeLoanStatusCommand {
                        LoanId = loanId,
                        StatusId = DefinedLoanStatuses.Defaulted.Id
                    });
                    Assert.True(resultDefaulted);

                    Assert.Equal(DefinedLoanStatuses.Defaulted.Id, loan.LoanStatusId);
                    Assert.Equal(DefinedApplicationStatuses.Defaulted.Id, application.ApplicationStatusId);
                    break;

                case DefinedLoanStatuses.Disbursed.Name:

                    var resultDisbursed = await _mediator.Send(new ChangeLoanStatusCommand {
                        LoanId = loanId,
                        StatusId = DefinedLoanStatuses.Disbursed.Id
                    });
                    Assert.True(resultDisbursed);

                    Assert.Equal(DefinedLoanStatuses.Disbursed.Id, loan.LoanStatusId);
                    Assert.Equal(DefinedApplicationStatuses.Disbursed.Id, application.ApplicationStatusId);
                    break;

                case DefinedLoanStatuses.Guaranted.Name:

                    var result = await _mediator.Send(new ChangeLoanStatusCommand {
                        LoanId = loanId,
                        StatusId = DefinedLoanStatuses.Guaranted.Id
                    });
                    Assert.True(result);

                    Assert.Equal(DefinedLoanStatuses.Guaranted.Id, loan.LoanStatusId);
                    Assert.Equal(DefinedApplicationStatuses.Guaranted.Id, application.ApplicationStatusId);
                    break;

                case DefinedLoanStatuses.Rejected.Name:

                    var resultRejected = await _mediator.Send(new ChangeLoanStatusCommand {
                        LoanId = loanId,
                        StatusId = DefinedLoanStatuses.Rejected.Id
                    });
                    Assert.True(resultRejected);

                    Assert.Equal(DefinedLoanStatuses.Rejected.Id, loan.LoanStatusId);
                    Assert.Equal(DefinedApplicationStatuses.Rejected.Id, application.ApplicationStatusId);
                    break;

                case DefinedLoanStatuses.Repaid.Name:

                    var resultRepaid = await _mediator.Send(new ChangeLoanStatusCommand {
                        LoanId = loanId,
                        StatusId = DefinedLoanStatuses.Repaid.Id
                    });
                    Assert.True(resultRepaid);

                    Assert.Equal(DefinedLoanStatuses.Repaid.Id, loan.LoanStatusId);
                    Assert.Equal(DefinedApplicationStatuses.Repaid.Id, application.ApplicationStatusId);
                    break;

                default:
                    throw new Exception();
            }
        }
    }
}
