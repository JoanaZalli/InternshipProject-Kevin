using Domain.Common;

namespace Domain.Entities {
    public class UserVerificationAndReset : BaseEntity {
        public Guid Id { get; set; }
        public string? EmailVerificationToken { get; set; }
        public DateTime? EmailVerificationTokenExpiry { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public string UserEmail { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
