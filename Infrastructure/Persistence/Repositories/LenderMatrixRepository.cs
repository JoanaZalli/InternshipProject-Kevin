using Application.Persistance;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories {
    public class LenderMatrixRepository : BaseRepository<LenderMatrix>, ILenderMatrixRepository {
        public LenderMatrixRepository(DatabaseContext databaseContext) : base(databaseContext) {
        }

        public async Task DeleteAsync(Guid productId, Guid lenderId) {
            var matrices = await _databaseContext.LenderMatrices
            .Where(x => x.LenderId == lenderId
                        && x.ProductId == productId
                        && x.IsDeleted != true) // to not mark as deleted dtwice
            .ToListAsync();

            matrices.ForEach(x => x.IsDeleted = true);
        }

        public async Task<bool> ContainsAsync(Guid lenderId, Guid productId) {
            var matrix = await _databaseContext.LenderMatrices
            .Where(x => x.LenderId == lenderId
                        && x.ProductId == productId && x.IsDeleted == false)
            .FirstOrDefaultAsync();

            return matrix != null;
        }

        public async Task<bool> ContainsAsync(Guid lenderId, Guid productId, int tenor) {
            var matrix = await _databaseContext.LenderMatrices
            .Where(x => x.LenderId == lenderId
                        && x.ProductId == productId
                        && x.Tenor == tenor
                        && x.IsDeleted == false)
            .FirstOrDefaultAsync();

            return matrix != null;
        }

        public async Task<LenderMatrix> GetByIdAsync(Guid lenderId, Guid productId, int tenor) {
            var matrix = await _databaseContext.LenderMatrices
            .Where(x => x.LenderId == lenderId
                        && x.ProductId == productId
                        && x.Tenor == tenor
                        && x.IsDeleted == false)
            .FirstOrDefaultAsync();

#pragma warning disable CS8603 // Possible null reference return.
            return matrix;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<List<LenderMatrix>> GetMatricesAsync(Guid lenderId, Guid productId) {
            var matrices = await _databaseContext.LenderMatrices
                .Where(x => x.LenderId == lenderId
                            && x.ProductId == productId
                            && x.IsDeleted == false)
                .ToListAsync();

            return matrices;
        }

        public async Task UpdateAsync(LenderMatrix entity) {
            var matrix = await _databaseContext.LenderMatrices
            .Where(x => x.LenderId == entity.LenderId
                        && x.ProductId == entity.ProductId
                        && x.Tenor == entity.Tenor
                        && x.IsDeleted == false)
            .FirstOrDefaultAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            matrix.Spread = entity.Spread;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public async Task CreateAsync(List<LenderMatrix> matrices) {
            foreach (var matrix in matrices)
                await base.CreateAsync(matrix);
        }
    }
}

