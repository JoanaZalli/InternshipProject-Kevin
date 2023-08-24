using Application.Persistance.Common;
using Domain.Entities;

namespace Application.Persistance {
    public interface ILoanStatusRepository : IBaseRepository<LoanStatus> {
        Task<bool> ContainsAsync(Guid id);
    }
}
