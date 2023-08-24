using Application.Persistance.Common;
using Domain.Common;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Common {
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity {

        protected readonly DatabaseContext _databaseContext;

        public BaseRepository(DatabaseContext databaseContext) {
            _databaseContext = databaseContext;
        }

        public async Task CreateAsync(T entity) {
            await _databaseContext.Set<T>().AddAsync(entity);
        }

        public async Task DeleteAsync(Guid id) {
            var toBeDeleted = await GetByIdAsync(id);
            var _ = _databaseContext.Set<T>().Remove(toBeDeleted);
        }

        public async Task<List<T>> GetAllAsync() {
            return await _databaseContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id) {
            var entity = await _databaseContext.Set<T>().FindAsync(id);
#pragma warning disable CS8603 // Possible null reference return.
            return entity;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public IQueryable<T> GetIQueryable() {
            return _databaseContext.Set<T>();
        }
    }
}
