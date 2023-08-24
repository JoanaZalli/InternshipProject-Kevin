namespace Application.UseCases.Authentication.Results {
    public class RegisterResult {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
