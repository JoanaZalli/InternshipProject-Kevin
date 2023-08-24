using Application.Persistance;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories {
    public class BorrowerRepository : BaseRepository<Borrower>, IBorrowerRepository {
        public BorrowerRepository(DatabaseContext databaseContext) : base(databaseContext) {
        }

        public async Task<bool> ContainsAsync(Guid id) {
            var borrower = await _databaseContext.Borrowers
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return borrower != null;
        }

        public new async Task<Borrower> GetByIdAsync(Guid id) {
            var borrower = await _databaseContext.Borrowers
                .Where(x => x.Id == id)
                .Include(x => x.User)
                .Include(x => x.CompanyProfile)
                .FirstOrDefaultAsync();
#pragma warning disable CS8603 // Possible null reference return.
            return borrower;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public IQueryable<Borrower> GetIQueryable(Guid userId) {
            var borrowers = _databaseContext.Borrowers
                .Include(x => x.User)
                .Where(x => x.UserId == userId)
                .AsQueryable();

            return borrowers;
        }

        public async Task<bool> HasApplicationsAsync(Guid id) {
            var borrower = await _databaseContext.Borrowers
                .Include(x => x.Applications)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var applications = borrower.Applications;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (applications is null || applications.Any() is false)
                return false;

            return true;
        }

        public async Task<bool> IsFiscalCodeUniqueAsync(Guid id, string fiscalCode) {
            var borrowers = await _databaseContext.Borrowers
                .Where(x => x.UserId == id)
                .ToListAsync();

            if (borrowers.Any() is false)
                return true;

            var flag = borrowers.Any(x => x.FiscalCode == fiscalCode);

            if (flag is false)
                return true;
            else
                return false;
        }

        public async Task<bool> UpdateAsync(Guid Id, Borrower borrower) {
            var entity = await base.GetByIdAsync(Id);
            entity.CompanyName = borrower.CompanyName;
            entity.CompanyType = borrower.CompanyType;
            entity.VATNumber = borrower.VATNumber;
            entity.FiscalCode = borrower.FiscalCode;
            return true;
        }
    }
}
