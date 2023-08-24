namespace InternshipProject.Objects.Requests.AuthenticationRequests {
    public class RegisterRequest {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Prefix { get; set; } = null!;
        public string Phone { get; set; } = null!;

    }
}
