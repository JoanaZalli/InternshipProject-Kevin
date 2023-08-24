using Application.Persistance.Common;
using Domain.Entities;

namespace Application.Persistance {
    public interface ILoanRepository : IBaseRepository<Loan> {
        Task<bool> ContainsAsync(Guid id);
        IQueryable<Loan> GetLoansByBorrower(Guid borrowerId);
        Task<Loan> GetLoanByBorrowerAsync(Guid borrowerId, Guid loanId);
        Task UpdateStatusAsync(Guid loanId, Guid statusId);
        Task<Loan> GetByIdWithApplicationAsync(Guid id);
        new Task<Loan> GetByIdAsync(Guid id);
        new IQueryable<Loan> GetIQueryable();
    }
}
