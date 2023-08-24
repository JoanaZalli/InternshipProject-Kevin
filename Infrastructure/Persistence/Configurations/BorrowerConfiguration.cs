using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations {
    public class BorrowerConfiguration : IEntityTypeConfiguration<Borrower> {
        public void Configure(EntityTypeBuilder<Borrower> builder) {
            builder
                .ToTable("Borrowers")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.CompanyName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.VATNumber)
                .IsRequired();

            builder
                .Property(x => x.FiscalCode)
                .IsRequired();

            builder
                 .HasMany(x => x.Applications)
                 .WithOne(y => y.Borrower)
                 .HasForeignKey(x => x.BorrowerId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.User)
                .WithMany(y => y.Borrowers)
                .HasForeignKey(x => x.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.CompanyType)
                .WithMany(y => y.Borrowers)
                .HasForeignKey(x => x.CompanyTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.CompanyProfile)
                .WithOne(y => y.Borrower)
                .HasForeignKey<CompanyProfile>(x => x.BorrowerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
