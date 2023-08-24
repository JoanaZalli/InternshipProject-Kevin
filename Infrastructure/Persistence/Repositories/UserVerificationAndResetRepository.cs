using Application.Persistance;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories {
    public class UserVerificationAndResetRepository : BaseRepository<UserVerificationAndReset>, IUserVerificationAndResetRepository {
        public UserVerificationAndResetRepository(DatabaseContext databaseContext) : base(databaseContext) {
        }

        public async Task<bool> ContainsEmailAsync(string email) {
            var entity = await _databaseContext.UserVerificationAndReset
                .Where(x => x.UserEmail == email)
                .FirstOrDefaultAsync();

            if (entity is null)
                return false;
            return true;
        }

        public async Task<bool> ContainsPasswordTokenAsync(string token) {
            var entity = await _databaseContext.UserVerificationAndReset
                .Where(x => x.PasswordResetToken == token)
                .FirstOrDefaultAsync();

            if (entity is null)
                return false;
            return true;
        }

        public async Task<bool> ContainsRefreshTokenAsync(string token) {
            var entity = await _databaseContext.UserVerificationAndReset
                .Where(x => x.RefreshToken == token)
                .FirstOrDefaultAsync();

            if (entity is null)
                return false;
            return true;
        }

        public async Task<bool> ContainsVerificationTokenAsync(string token) {
            var entity = await _databaseContext.UserVerificationAndReset
                .Where(x => x.EmailVerificationToken == token)
                .FirstOrDefaultAsync();

            if (entity is null)
                return false;
            return true;
        }

        public async Task<UserVerificationAndReset> GetByEmailAsync(string email) {
            var entity = await _databaseContext.UserVerificationAndReset
                .Where(x => x.UserEmail == email)
                .FirstOrDefaultAsync();
#pragma warning disable CS8603 // Possible null reference return.
            return entity;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<bool> SetPasswordTokenAsync(string email, string passwordToken, DateTime tokenExpiry) {
            var entity = await GetByEmailAsync(email);

            if (entity is null)
                return false;

            entity.PasswordResetToken = passwordToken;
            entity.PasswordResetTokenExpiry = tokenExpiry;
            return true;
        }

        public async Task<bool> UpdateVerificationTokenAsync(string email, string verificationToken, DateTime tokenExpiry) {
            var entity = await GetByEmailAsync(email);

            if (entity is null)
                return false;

            entity.EmailVerificationToken = verificationToken;
            entity.EmailVerificationTokenExpiry = tokenExpiry;
            return true;
        }
    }
}
