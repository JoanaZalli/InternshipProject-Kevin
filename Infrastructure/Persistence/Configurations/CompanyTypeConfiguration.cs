using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CompanyType = Domain.Entities.CompanyType;

namespace Infrastructure.Persistence.Configurations {
    public class CompanyTypeConfiguration : IEntityTypeConfiguration<CompanyType> {
        public void Configure(EntityTypeBuilder<CompanyType> builder) {
            builder
                .ToTable("CompanyTypes")
                .HasKey(x => x.Id);

            builder
                .HasAlternateKey(x => x.Type);

            builder
                .Property(x => x.Type)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.Description)
                .HasMaxLength(100)
                .IsRequired(false);

            builder
                .HasMany(x => x.Borrowers)
                .WithOne(y => y.CompanyType)
                .HasForeignKey(x => x.CompanyTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
