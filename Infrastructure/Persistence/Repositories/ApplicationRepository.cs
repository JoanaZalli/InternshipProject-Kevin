using Application.Persistance;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories {
    public class ApplicationRepository : BaseRepository<ApplicationEntity>, IApplicationRepository {
        public ApplicationRepository(DatabaseContext databaseContext) : base(databaseContext) {
        }

        public async Task<bool> ContainsAsync(Guid id) {
            var application = await _databaseContext.Applications
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return application != null;
        }

        public async Task<ApplicationEntity> GetApplicationByBorrowerAsync(Guid borrowerId, Guid applicationId) {
            var application = await _databaseContext.Applications
                .Include(x => x.ApplicationStatus)
                .Where(x => x.BorrowerId == borrowerId
                            && x.Id == applicationId)
                .FirstOrDefaultAsync();

#pragma warning disable CS8603 // Possible null reference return.
            return application;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public IQueryable<ApplicationEntity> GetIQueryable(Guid borrowerId) {
            var applications = _databaseContext.Applications
                .Include(x => x.ApplicationStatus)
                .Where(x => x.BorrowerId == borrowerId)
                .AsQueryable();

            return applications;
        }

        public new IQueryable<ApplicationEntity> GetIQueryable() {
            var applications = _databaseContext.Applications
                .Include(x => x.ApplicationStatus)
                .AsQueryable();

            return applications;
        }

        public async Task<ApplicationEntity> GetWithProductAsync(Guid id) {
            var application = await _databaseContext.Applications
                .Include(x => x.Product)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

#pragma warning disable CS8603 // Possible null reference return.
            return application;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<string> GetCompanyTypeAsync(Guid id) {
            var application = await _databaseContext.Applications
                .Include(x => x.Borrower)
                .ThenInclude(x => x.CompanyType)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var companyType = application.Borrower.CompanyType.Type;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return companyType;
        }

        public async Task UpdateAsync(ApplicationEntity entity) {
            var application = await base.GetByIdAsync(entity.Id);
            application.Product = entity.Product;
            application.RequestedAmount = entity.RequestedAmount;
            application.RequestedTenor = entity.RequestedTenor;
            application.FinancePurposeDefinition = entity.FinancePurposeDefinition;
            application.Name = entity.Name;
        }

        public async Task<bool> IsApprovedAsLoanAsync(Guid id) {
            var application = await _databaseContext.Applications
                .Include(x => x.Loan)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return application.Loan != null;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public async Task UpdateStatusAsync(Guid applicationId, Guid statusId) {
            var application = await _databaseContext.Applications
                .Where(x => x.Id == applicationId)
                .FirstOrDefaultAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            application.ApplicationStatusId = statusId;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public new async Task<ApplicationEntity> GetByIdAsync(Guid id) {
            var application = await _databaseContext.Applications
                .Include(x => x.ApplicationStatus)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

#pragma warning disable CS8603 // Possible null reference return.
            return application;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
