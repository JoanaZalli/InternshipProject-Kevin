namespace InternshipProject.Objects.Requests.AuthenticationRequests {
    public class RefreshTokenRequest {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
