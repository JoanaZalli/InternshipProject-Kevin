using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations {
    public class ApplicationStatusConfiguration : IEntityTypeConfiguration<ApplicationStatus> {
        public void Configure(EntityTypeBuilder<ApplicationStatus> builder) {
            builder
                .ToTable("ApplicationStatuses")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .HasMany(x => x.Applications)
                .WithOne(x => x.ApplicationStatus)
                .HasForeignKey(x => x.ApplicationStatusId)
                .IsRequired();
        }
    }
}
