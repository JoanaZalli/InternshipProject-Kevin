using Application.Persistance.Common;
using Domain.Entities;

namespace Application.Persistance {
    public interface IPermissionRepository : IBaseRepository<Permission> {
        Task<bool> ContainsAsync(Guid id);
    }
}
