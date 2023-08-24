using Application.Persistance.Common;
using Domain.Entities;

namespace Application.Persistance {
    public interface IApplicationRepository : IBaseRepository<ApplicationEntity> {
        new Task<ApplicationEntity> GetByIdAsync(Guid id);
        Task<bool> ContainsAsync(Guid id);
        Task UpdateAsync(ApplicationEntity entity);
        Task<ApplicationEntity> GetApplicationByBorrowerAsync(Guid borrowerId, Guid applicationId);
        Task<string> GetCompanyTypeAsync(Guid id);
        Task<ApplicationEntity> GetWithProductAsync(Guid id);
        Task<bool> IsApprovedAsLoanAsync(Guid id);
        Task UpdateStatusAsync(Guid applicationId, Guid statusId);
        IQueryable<ApplicationEntity> GetIQueryable(Guid borrowerId);
        new IQueryable<ApplicationEntity> GetIQueryable();
    }
}
