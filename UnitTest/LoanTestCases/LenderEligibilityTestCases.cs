using Application.Persistance;
using Application.Persistance.Common;
using Application.UseCases.LoanJourney.Commands;
using Domain.Entities;
using InternshipProject.Localizations;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTest.LoanTestCases {
    public class LenderEligibilityTestCases {

        private readonly Mock<IApplicationRepository> _applicationRepositoryMock;
        private readonly Mock<ILenderRepository> _lenderRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IStringLocalizer<LocalizationResources>> _localizationMock;
        private readonly Mock<ILenderMatrixRepository> _lenderMatrixRepositoryMock;
        private readonly Mock<ILoanRepository> _loanRepositoryMock;
        private readonly Mock<ILoanStatusRepository> _loanStatusRepositoryMock;
        private readonly Mock<ILogger<CreateLoanCommandHandler>> _loggerMock;

        public LenderEligibilityTestCases() {
            _applicationRepositoryMock = new();
            _lenderRepositoryMock = new();
            _unitOfWorkMock = new();
            _localizationMock = new();
            _lenderMatrixRepositoryMock = new();
            _loanRepositoryMock = new();
            _loanStatusRepositoryMock = new();
            _loggerMock = new();
        }

        [Theory]
        [InlineData(5)]
        [InlineData(66)]
        public async void IsEligible_TenorNotInLimits_ReturnsFalse(int requestTenor) {
            // Arrange
            var handler = new CreateLoanCommandHandler(_localizationMock.Object,
                                                       _unitOfWorkMock.Object,
                                                       _lenderRepositoryMock.Object,
                                                       _applicationRepositoryMock.Object,
                                                       _lenderMatrixRepositoryMock.Object,
                                                       _loanRepositoryMock.Object,
                                                       _loanStatusRepositoryMock.Object,
                                                       _loggerMock.Object);

            var lender = new Lender {
                MaxTenor = 65,
                MinTenor = 10
            };

            var application = new ApplicationEntity {
                RequestedTenor = requestTenor,
            };

            // Act
            var result = await handler.IsEligible(lender, application);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(65)]
        [InlineData(30)]
        public async void IsEligible_TenorInLimits_ReturnsTrue(int requestTenor) {
            // Arrange
            var handler = new CreateLoanCommandHandler(_localizationMock.Object,
                                                       _unitOfWorkMock.Object,
                                                       _lenderRepositoryMock.Object,
                                                       _applicationRepositoryMock.Object,
                                                       _lenderMatrixRepositoryMock.Object,
                                                       _loanRepositoryMock.Object,
                                                       _loanStatusRepositoryMock.Object,
                                                       _loggerMock.Object);

            var lender = new Lender {
                MaxTenor = 65,
                MinTenor = 10
            };

            var application = new ApplicationEntity {
                RequestedTenor = requestTenor,
            };

            // Act
            var result = await handler.IsEligible(lender, application);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(499999)]
        [InlineData(400000)]
        public async void IsEligible_SmallerRequestAmount_ReturnsFalse(int requestAmount) {
            // Arrange
            var handler = new CreateLoanCommandHandler(_localizationMock.Object,
                                                       _unitOfWorkMock.Object,
                                                       _lenderRepositoryMock.Object,
                                                       _applicationRepositoryMock.Object,
                                                       _lenderMatrixRepositoryMock.Object,
                                                       _loanRepositoryMock.Object,
                                                       _loanStatusRepositoryMock.Object,
                                                       _loggerMock.Object);

            var lender = new Lender {
                RequestedAmount = 500000
            };

            var application = new ApplicationEntity {
                RequestedAmount = requestAmount
            };

            // Act
            var result = await handler.IsEligible(lender, application);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(500000)]
        public async void IsEligible_CorrectRequestAmount_ReturnsTrue(int requestAmount) {
            // Arrange
            var handler = new CreateLoanCommandHandler(_localizationMock.Object,
                                                       _unitOfWorkMock.Object,
                                                       _lenderRepositoryMock.Object,
                                                       _applicationRepositoryMock.Object,
                                                       _lenderMatrixRepositoryMock.Object,
                                                       _loanRepositoryMock.Object,
                                                       _loanStatusRepositoryMock.Object,
                                                       _loggerMock.Object);

            var lender = new Lender {
                RequestedAmount = 500000
            };

            var application = new ApplicationEntity {
                RequestedAmount = requestAmount
            };

            // Act
            var result = await handler.IsEligible(lender, application);

            // Assert
            Assert.True(result);
        }
    }
}
