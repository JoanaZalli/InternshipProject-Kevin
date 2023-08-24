using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations {
    public class ApplicationConfiguration : IEntityTypeConfiguration<ApplicationEntity> {
        public void Configure(EntityTypeBuilder<ApplicationEntity> builder) {
            builder
                .ToTable("Applications")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.RequestedAmount)
                .IsRequired();

            builder
                .Property(x => x.RequestedTenor)
                .IsRequired();

            builder
                .Property(x => x.FinancePurposeDefinition)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .HasOne(x => x.Product)
                .WithMany(y => y.Applications)
                .HasForeignKey(x => x.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.Loan)
                .WithOne(y => y.Application)
                .HasForeignKey<Loan>(x => x.ApplicationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(a => a.Borrower)
                .WithMany(b => b.Applications)
                .IsRequired(false);

            builder
                .HasOne(x => x.ApplicationStatus)
                .WithMany(x => x.Applications)
                .HasForeignKey(x => x.ApplicationStatusId)
                .IsRequired();
        }
    }
}
