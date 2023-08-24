using Application.Persistance.Common;
using Domain.Entities;

namespace Application.Persistance {
    public interface ICompanyTypeRepository : IBaseRepository<CompanyType> {
        Task<bool> ContainsAsync(Guid id);
    }
}
