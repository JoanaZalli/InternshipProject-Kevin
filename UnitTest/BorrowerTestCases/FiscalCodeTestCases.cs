using Application.UseCases.BorrowerJourney.Commands;
using Domain.Seeds;

namespace UnitTest.BorrowerTestCases {
    public class FiscalCodeTestCases {


        public FiscalCodeTestCases() {

        }

        [Theory]
        [InlineData("1111111111111111")]
        public void FiscalCodeIsValid_SoleProprietorship_CorrectFiscalCode_ReturnsTrue(string fiscalCode) {
            // Arrange
            var soleProprietorshipId = DefinedCompanyTypes.SoleProprietorship.Id;

            // Act
            var result = CreateBorrowerCommandHandler.IsValid(soleProprietorshipId, fiscalCode);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("11111111111111111")]
        public void FiscalCodeIsValid_SoleProprietorship_WrongFiscalCode_ReturnsFalse(string fiscalCode) {
            // Arrange
            var soleProprietorshipId = DefinedCompanyTypes.SoleProprietorship.Id;

            // Act
            var result = CreateBorrowerCommandHandler.IsValid(soleProprietorshipId, fiscalCode);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("11111111111")]
        public void FiscalCodeIsValid_Other_CorrectFiscalCode_ReturnsTrue(string fiscalCode) {
            // Arrange
            var otherId = DefinedCompanyTypes.Other.Id;

            // Act
            var result = CreateBorrowerCommandHandler.IsValid(otherId, fiscalCode);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("11111111111111111")]
        public void FiscalCodeIsValid_Other_WrongFiscalCode_ReturnsFalse(string fiscalCode) {
            // Arrange
            var otherId = DefinedCompanyTypes.Other.Id;

            // Act
            var result = CreateBorrowerCommandHandler.IsValid(otherId, fiscalCode);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("11111111111111111")]
        public void FiscalCodeIsValid_AnyCompany_ReturnsTrue(string fiscalCode) {
            // Arrange
            var cooperativeSocietyId = DefinedCompanyTypes.CooperativeSociety.Id;

            // Act
            var result = CreateBorrowerCommandHandler.IsValid(cooperativeSocietyId, fiscalCode);

            // Assert
            Assert.True(result);
        }
    }
}
