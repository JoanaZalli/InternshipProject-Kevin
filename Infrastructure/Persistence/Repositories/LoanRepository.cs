using Application.Persistance;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories {
    public class LoanRepository : BaseRepository<Loan>, ILoanRepository {
        public LoanRepository(DatabaseContext databaseContext) : base(databaseContext) {
        }

        public async Task<bool> ContainsAsync(Guid id) {
            var loan = await _databaseContext.Loans
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return loan != null;
        }

        public async Task<Loan> GetByIdWithApplicationAsync(Guid id) {
            var loan = await _databaseContext.Loans
                .Include(x => x.Application)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

#pragma warning disable CS8603 // Possible null reference return.
            return loan;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<Loan> GetLoanByBorrowerAsync(Guid borrowerId, Guid loanId) {
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var borrower = await _databaseContext.Borrowers
                .Include(x => x.Applications)
                .ThenInclude(x => x.Loan)
                .ThenInclude(x => x.LoanStatus)
                .Where(x => x.Id == borrowerId)
                .FirstOrDefaultAsync();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

            var applications = borrower.Applications;
            foreach (var application in applications)
                if (application.Loan is not null && application.Loan.Id == loanId)
                    return application.Loan;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public IQueryable<Loan> GetLoansByBorrower(Guid borrowerId) {
            var loans = _databaseContext.Loans
                .Include(x => x.Application)
                .ThenInclude(x => x.Borrower)
                .Include(x => x.LoanStatus)
                .AsQueryable();

            loans = loans.Where(x => x.Application.Borrower.Id == borrowerId);
            return loans;
        }

        public async Task UpdateStatusAsync(Guid loanId, Guid statusId) {
            var loan = await _databaseContext.Loans
                .Where(x => x.Id == loanId)
                .FirstOrDefaultAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            loan.LoanStatusId = statusId;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public new IQueryable<Loan> GetIQueryable() {
            var loans = _databaseContext.Loans
                .Include(x => x.LoanStatus)
                .AsQueryable();

            return loans;
        }

        public new async Task<Loan> GetByIdAsync(Guid id) {
            var loan = await _databaseContext.Loans
                .Include(x => x.LoanStatus)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

#pragma warning disable CS8603 // Possible null reference return.
            return loan;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
