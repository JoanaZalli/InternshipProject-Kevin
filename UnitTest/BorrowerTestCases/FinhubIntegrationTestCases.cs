using Application.Exceptions.ServerErrors;
using Application.UseCases.BorrowerJourney.Commands;
using Application.UseCases.Common;

namespace UnitTest.BorrowerTestCases {
    public class FinhubIntegrationTestCases {

        private readonly FinHubConnectionSettings _fibHubSettings;

        public FinhubIntegrationTestCases() {
            _fibHubSettings = new FinHubConnectionSettings {
                AuthToken = "cj3rfthr01qlttl4igc0cj3rfthr01qlttl4igcg"
            };
        }

        [Theory]
        [InlineData("AAPL")]
        public async void EstablishFinhubConnection_HappyPath_ReturnsCompanyProfile(string ticketName) {

            // Arrange
            var token = _fibHubSettings.AuthToken;

            // Act
            var result = await CreateBorrowerCommandHandler.GetCompanyProfile(ticketName, token);

            // Assert
            Assert.NotNull(result.Name);
        }

        [Theory]
        [InlineData("AAPL")]
        public async void EstablishFinhubConnection_InvalidToken_ThrowsThirdPartyException(string ticketName) {

            // Arrange
            var token = _fibHubSettings.AuthToken.Replace('0', '8');

            // Act & Assert
            await Assert.ThrowsAsync<ThirdPartyException>(async () => await CreateBorrowerCommandHandler.GetCompanyProfile(ticketName, token));
        }

        [Theory]
        [InlineData("DONTEXIST")]
        public async void EstablishFinhubConnection_InvalidCompanyName_ReturnsNull(string ticketName) {

            // Arrange
            var token = _fibHubSettings.AuthToken;

            // Act
            var result = await CreateBorrowerCommandHandler.GetCompanyProfile(ticketName, token);

            // Assert
            Assert.Null(result.Name);
        }

    }
}


