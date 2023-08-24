using Application.Persistance.Common;
using Domain.Entities;

namespace Application.Persistance {
    public interface ILenderMatrixRepository : IBaseRepository<LenderMatrix> {
        Task CreateAsync(List<LenderMatrix> entities);
        Task DeleteAsync(Guid productId, Guid lenderId);
        Task UpdateAsync(LenderMatrix entity);
        Task<bool> ContainsAsync(Guid lenderId, Guid productId);
        Task<bool> ContainsAsync(Guid lenderId, Guid productId, int tenor);
        Task<List<LenderMatrix>> GetMatricesAsync(Guid lenderId, Guid productId);
        Task<LenderMatrix> GetByIdAsync(Guid lenderId, Guid productId, int tenor);
    }
}
