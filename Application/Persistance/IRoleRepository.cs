using Application.Persistance.Common;
using Domain.Entities;

namespace Application.Persistance {
    public interface IRoleRepository : IBaseRepository<Role> {
        IQueryable<Permission> GetPermissions(Guid id);
        Task<bool> ClearPermissionsAsync(Guid id);
        Task<bool> UpdatePermissionsAsync(Guid id, IEnumerable<Permission> permissions);
        Task<bool> ContainsAsync(Guid id);
    }
}
