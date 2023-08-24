namespace Infrastructure.Services.Common {
    public class JwtSettings {
        public const string SectionName = "JwtSettings";
        public string Secret { get; set; } = null!;
        public int ExpiryDays { get; set; }
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
    }
}
