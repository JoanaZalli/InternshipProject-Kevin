using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations {
    public class LoanStatusConfiguration : IEntityTypeConfiguration<LoanStatus> {
        public void Configure(EntityTypeBuilder<LoanStatus> builder) {
            builder
                .ToTable("LoanStatuses")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .HasMany(x => x.Loans)
                .WithOne(x => x.LoanStatus)
                .HasForeignKey(x => x.LoanStatusId)
                .IsRequired();
        }
    }
}
