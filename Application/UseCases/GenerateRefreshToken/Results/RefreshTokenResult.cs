namespace Application.UseCases.GenerateRefreshToken.Results {
    public class RefreshTokenResult {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
