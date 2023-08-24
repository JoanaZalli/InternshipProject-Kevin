using Application.Persistance;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories {
    public class RoleRepository : BaseRepository<Role>, IRoleRepository {
        public RoleRepository(DatabaseContext databaseContext) : base(databaseContext) {
        }

        public async Task<bool> ClearPermissionsAsync(Guid id) {
            var role = await _databaseContext.Roles
                .Include(x => x.Permissions)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (role is null)
                return false;

            role.Permissions.Clear();
            return true;
        }

        public async Task<bool> ContainsAsync(Guid id) {
            var entity = await _databaseContext.Roles
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (entity is null)
                return false;
            return true;
        }

        public IQueryable<Permission> GetPermissions(Guid id) {
            var permissions = _databaseContext.Roles
                .Include(x => x.Permissions)
                .Where(x => x.Id == id)
                .SelectMany(x => x.Permissions)
                .AsQueryable();

            return permissions;
        }

        public async Task<bool> UpdatePermissionsAsync(Guid id, IEnumerable<Permission> permissions) {
            var role = await _databaseContext.Roles
                .Include(x => x.Permissions)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (role is null)
                return false;

            role.Permissions.Clear();

            foreach (var item in permissions)
                role.Permissions.Add(item);
            return true;
        }
    }
}
