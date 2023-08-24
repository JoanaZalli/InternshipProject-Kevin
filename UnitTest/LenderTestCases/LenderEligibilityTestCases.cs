using Application.UseCases.LenderCases.Queries;
using Domain.Entities;
using Domain.Seeds;

namespace UnitTest.LenderTestCases {
    public class LenderEligibilityTestCases {
        public LenderEligibilityTestCases() {

        }

        [Theory]
        [InlineData(400000)]
        public void IsRequestAmount_SmallerThanRequired_ReturnsFalse(int amount) {
            // Arrange
            var lender = new Lender {
                RequestedAmount = 500000
            };

            // Act
            var result = GetEligibleLendersQueryHandler.IsRequestAmountAccepted(lender, amount);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(550000)]
        public void IsRequestAmount_CorrectAmount_ReturnsTrue(int amount) {
            // Arrange
            var lender = new Lender {
                RequestedAmount = 500000
            };

            // Act
            var result = GetEligibleLendersQueryHandler.IsRequestAmountAccepted(lender, amount);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(5)]
        public void IsTenorValid_SmallerThanRequired_ReturnsFalse(int tenor) {
            // Arrange
            var lender = new Lender {
                MaxTenor = 60,
                MinTenor = 10,
            };

            // Act
            var result = GetEligibleLendersQueryHandler.IsTenorAccepted(lender, tenor);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(65)]
        public void IsTenorValid_BiggerThanRequired_ReturnsFalse(int tenor) {
            // Arrange
            var lender = new Lender {
                MaxTenor = 60,
                MinTenor = 10,
            };

            // Act
            var result = GetEligibleLendersQueryHandler.IsTenorAccepted(lender, tenor);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(30)]
        [InlineData(60)]
        [InlineData(10)]
        public void IsTenorValid_BetweenLimits_ReturnsTrue(int tenor) {
            // Arrange
            var lender = new Lender {
                MaxTenor = 60,
                MinTenor = 10,
            };

            // Act
            var result = GetEligibleLendersQueryHandler.IsTenorAccepted(lender, tenor);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(DefinedCompanyTypes.SoleProprietorship.Name)]
        public void IsCompanyTypeValid_SameCompany_ReturnsTrue(string companyType) {
            // Arrange
            var lender = new Lender {
                BorrowerCompanyType = DefinedCompanyTypes.SoleProprietorship.Name
            };

            // Act
            var result = GetEligibleLendersQueryHandler.IsCompanyTypeAccepted(lender, companyType);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(DefinedCompanyTypes.SoleProprietorship.Name)]
        public void IsCompanyTypeValid_EmptyCompany_ReturnsTrue(string companyType) {
            // Arrange
            var lender = new Lender {
                BorrowerCompanyType = ""
            };

            // Act
            var result = GetEligibleLendersQueryHandler.IsCompanyTypeAccepted(lender, companyType);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(DefinedCompanyTypes.SoleProprietorship.Name)]
        public void IsCompanyTypeValid_NullCompany_ReturnsTrue(string companyType) {
            // Arrange
            var lender = new Lender {
                BorrowerCompanyType = null!
            };

            // Act
            var result = GetEligibleLendersQueryHandler.IsCompanyTypeAccepted(lender, companyType);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(DefinedCompanyTypes.SoleProprietorship.Name)]
        public void IsCompanyTypeValid_DifferentCompany_ReturnsTrue(string companyType) {
            // Arrange
            var lender = new Lender {
                BorrowerCompanyType = DefinedCompanyTypes.GeneralPartnership.Name
            };

            // Act
            var result = GetEligibleLendersQueryHandler.IsCompanyTypeAccepted(lender, companyType);

            // Assert
            Assert.False(result);
        }
    }
}
