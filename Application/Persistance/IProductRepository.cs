using Application.Persistance.Common;
using Domain.Entities;

namespace Application.Persistance {
    public interface IProductRepository : IBaseRepository<Product> {
        Task<bool> ContainsAsync(Guid id);
    }
}
