namespace Application.Persistance.Common {
    public interface IBaseRepository<T> {
        Task<List<T>> GetAllAsync();
        Task CreateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task<T> GetByIdAsync(Guid id);
        IQueryable<T> GetIQueryable();
    }
}
