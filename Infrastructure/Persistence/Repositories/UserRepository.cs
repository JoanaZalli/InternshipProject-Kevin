using Application.Persistance;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories {
    public class UserRepository : BaseRepository<User>, IUserRepository {
        public UserRepository(DatabaseContext databaseContext) : base(databaseContext) {
        }

        public async Task ActivateAccountAsync(string email) {
            var entity = await GetByEmailAsync(email);
            entity.IsEmailConfirmed = true;
        }

        public async Task<bool> ContainsEmailAsync(string email) {
            var entity = await _databaseContext.Users
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync();

            if (entity is null)
                return false;
            return true;
        }

        public async Task<bool> ContainsUsernameAsync(string username) {
            var entity = await _databaseContext.Users
                .Where(x => x.Username == username)
                .FirstOrDefaultAsync();

            if (entity is null)
                return false;
            return true;
        }

        public async Task<User> GetByUsernameAsync(string username) {
            var entity = await _databaseContext.Users
                .Where(x => x.Username == username)
                .FirstOrDefaultAsync();
#pragma warning disable CS8603 // Possible null reference return.
            return entity;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<User> GetByEmailAsync(string email) {
            var entity = await _databaseContext.Users
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync();

            if (entity is null)
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.

            return entity;
        }

        public async Task<HashSet<string>> GetPermissionsAsync(Guid userId) {
            var roles = await _databaseContext.Users
                .Include(x => x.Roles)
                .ThenInclude(x => x.Permissions)
                .Where(x => x.Id == userId)
                .Select(x => x.Roles)
                .ToArrayAsync();

            var permissions = roles
                .SelectMany(x => x)
                .SelectMany(x => x.Permissions)
                .Select(x => x.Name)
                .ToHashSet();

            return permissions;
        }

        public async Task<bool> ChangePasswordAsync(string email, string passwordHash, string passwordSalt) {
            var entity = await GetByEmailAsync(email);

            if (entity is null)
                return false;

            entity.PasswordHash = passwordHash;
            entity.PasswordSalt = passwordSalt;
            return true;
        }

        public async Task<IEnumerable<Role>> GetRolesAsync(Guid userId) {
            var roles = await _databaseContext.Users
                .Include(x => x.Roles)
                .Where(x => x.Id == userId)
                .SelectMany(x => x.Roles)
                .ToListAsync();

            return roles.AsEnumerable();
        }

        public async Task<bool> UpdateRolesAsync(Guid id, IEnumerable<Role> roles) {
            var user = await _databaseContext.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user is null)
                return false;

            user.Roles.Clear();

            foreach (var role in roles)
                user.Roles.Add(role);
            return true;
        }

        public async Task<bool> ClearRolesAsync(Guid id) {
            var user = await _databaseContext.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user is null)
                return false;

            user.Roles.Clear();
            return true;
        }

        public async Task<bool> AddRoleAsync(Guid id, Role role) {
            var user = await _databaseContext.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user is null)
                return false;

            user.Roles.Add(role);
            return true;
        }

        public async Task<bool> SetRefreshTokenAsync(Guid id, string token, DateTime time) {
            var user = await _databaseContext.Users
                .Include(x => x.UserVerificationAndReset)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user is null)
                return false;

            user.UserVerificationAndReset = new UserVerificationAndReset {
                RefreshToken = token,
                RefreshTokenExpiry = time,
                UserEmail = user.Email
            };
            return true;
        }

        public async Task<bool> ContainsAsync(Guid id) {
            var entity = await _databaseContext.Users
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (entity is null)
                return false;
            return true;
        }

        public async Task ResetTriesAsync(Guid id) {
            var entity = await _databaseContext.Users
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            entity.LoginTries = 0;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public async Task<bool> IncrementTriesAsync(Guid id) {
            var entity = await _databaseContext.Users
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (entity.LoginTries == 3)
                return false;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            entity.LoginTries += 1;
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task BlockAccountAsync(Guid id) {
            var entity = await _databaseContext.Users
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            entity.IsBlocked = true;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            await _databaseContext.SaveChangesAsync();
        }

        public async Task UnblockAccountAsync(Guid id) {
            var entity = await _databaseContext.Users
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            entity.IsBlocked = false;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public IQueryable<User> GetBlockedAccountsAsync() {
            var entities = _databaseContext.Users
                .Where(x => x.IsBlocked == true)
                .AsQueryable();

            return entities;
        }

        public async Task<bool> IsAccountActivatedAsync(string email) {
            var entity = await _databaseContext.Users
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (entity.IsEmailConfirmed is false)
                return false;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return true;
        }

        public async Task<bool> HasBorrowersAsync(Guid id) {
            var user = await _databaseContext.Users
                .Include(x => x.Borrowers)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var borrowers = user.Borrowers;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (borrowers is null || borrowers.Any() is false)
                return false;

            return true;
        }

        public IQueryable<Role> GetRoles(Guid userId) {
            var roles = _databaseContext.Users
                .Include(x => x.Roles)
                .Where(x => x.Id == userId)
                .SelectMany(x => x.Roles)
                .AsQueryable();

            return roles;
        }
    }
}
