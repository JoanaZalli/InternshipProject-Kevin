using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations {
    public class LenderMatrixConfiguration : IEntityTypeConfiguration<LenderMatrix> {
        public void Configure(EntityTypeBuilder<LenderMatrix> builder) {
            builder
                .ToTable("LenderMatrices")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.LenderId)
                .IsRequired();

            builder
                .Property(x => x.ProductId)
                .IsRequired();

            builder
                .Property(x => x.Tenor)
                .IsRequired();

            builder
                .Property(x => x.Spread)
                .HasPrecision(3, 2) // 0.02 -> 2%
                .IsRequired();
        }
    }
}
