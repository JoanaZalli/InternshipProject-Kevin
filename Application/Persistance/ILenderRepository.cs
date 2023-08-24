using Application.Persistance.Common;
using Domain.Entities;

namespace Application.Persistance {
    public interface ILenderRepository : IBaseRepository<Lender> {
        Task<bool> ContainsAsync(Guid id);
    }
}
