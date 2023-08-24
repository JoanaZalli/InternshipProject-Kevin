using Application.Persistance;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository {
        public PermissionRepository(DatabaseContext databaseContext) : base(databaseContext) {
        }

        public async Task<bool> ContainsAsync(Guid id) {
            var entity = await _databaseContext.Permissions
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (entity is null)
                return false;
            return true;
        }
    }
}
