using Application.Persistance.Common;
using Domain.Entities;

namespace Application.Persistance {
    public interface IUserRepository : IBaseRepository<User> {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<HashSet<string>> GetPermissionsAsync(Guid userId);
        Task<IEnumerable<Role>> GetRolesAsync(Guid userId);
        IQueryable<Role> GetRoles(Guid userId);
        Task<bool> ContainsUsernameAsync(string username);
        Task<bool> ContainsAsync(Guid id);
        Task<bool> ContainsEmailAsync(string email);
        Task ActivateAccountAsync(string email);
        Task<bool> IsAccountActivatedAsync(string email);
        Task<bool> ChangePasswordAsync(string email, string passwordSalt, string passwordHash);
        Task<bool> AddRoleAsync(Guid id, Role role);
        Task<bool> UpdateRolesAsync(Guid id, IEnumerable<Role> roles);
        Task<bool> ClearRolesAsync(Guid id);
        Task<bool> SetRefreshTokenAsync(Guid id, string token, DateTime time);
        Task ResetTriesAsync(Guid id);
        Task<bool> IncrementTriesAsync(Guid id);
        Task BlockAccountAsync(Guid id);
        Task UnblockAccountAsync(Guid id);
        IQueryable<User> GetBlockedAccountsAsync();
        Task<bool> HasBorrowersAsync(Guid id);
    }
}
