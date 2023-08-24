using Application.Persistance;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories.Common;

namespace Infrastructure.Persistence.Repositories {
    public class ApplicationStatusRepository : BaseRepository<ApplicationStatus>, IApplicationStatusRepository {
        public ApplicationStatusRepository(DatabaseContext databaseContext) : base(databaseContext) {
        }
    }
}
