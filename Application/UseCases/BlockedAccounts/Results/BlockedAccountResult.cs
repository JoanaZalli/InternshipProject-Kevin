namespace Application.UseCases.BlockedAccounts.Results {
    public class BlockedAccountResult {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Prefix { get; set; } = null!;
    }
}
