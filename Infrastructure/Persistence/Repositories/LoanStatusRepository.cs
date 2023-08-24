using Application.Persistance;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories {
    public class LoanStatusRepository : BaseRepository<LoanStatus>, ILoanStatusRepository {
        public LoanStatusRepository(DatabaseContext databaseContext) : base(databaseContext) {
        }

        public async Task<bool> ContainsAsync(Guid id) {
            var status = await _databaseContext.LoanStatuses
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return status != null;
        }
    }
}
