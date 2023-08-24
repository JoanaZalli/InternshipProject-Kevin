namespace InternshipProject.Objects.Requests.AuthenticationRequests {
    public class ResetPasswordRequest {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
}
