using Application.Persistance.Common;
using Domain.Entities;

namespace Application.Persistance {
    public interface IUserVerificationAndResetRepository : IBaseRepository<UserVerificationAndReset> {
        Task<UserVerificationAndReset> GetByEmailAsync(string email);
        Task<bool> ContainsEmailAsync(string email);
        Task<bool> UpdateVerificationTokenAsync(string email, string verificationToken, DateTime tokenExpiry);
        Task<bool> ContainsVerificationTokenAsync(string token);
        Task<bool> SetPasswordTokenAsync(string email, string passwordToken, DateTime tokenExpiry);
        Task<bool> ContainsPasswordTokenAsync(string token);
        Task<bool> ContainsRefreshTokenAsync(string token);
    }
}
