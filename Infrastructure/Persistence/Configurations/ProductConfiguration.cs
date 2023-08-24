using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations {
    public class ProductConfiguration : IEntityTypeConfiguration<Product> {
        public void Configure(EntityTypeBuilder<Product> builder) {
            builder
                .ToTable("Products")
                .HasKey(x => x.Id);

            builder
                .HasAlternateKey(x => x.Name);

            builder
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.Description)
                .HasMaxLength(150)
                .IsRequired();

            builder
                .Property(x => x.ReferenceRate)
                .HasPrecision(18, 4)
                .IsRequired();

            builder
                .Property(x => x.FinanceMaxAmount)
                .HasPrecision(18, 4)
                .IsRequired();

            builder
                .Property(x => x.FinanceMinAmount)
                .HasPrecision(18, 4)
                .IsRequired();

            builder
                .HasMany(x => x.Applications)
                .WithOne(y => y.Product)
                .HasForeignKey(x => x.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
