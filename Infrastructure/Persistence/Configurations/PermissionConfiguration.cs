using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations {
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission> {
        public void Configure(EntityTypeBuilder<Permission> builder) {
            builder
                .ToTable("Permissions")
                .HasKey(x => x.Id);

            builder
                .HasAlternateKey(x => x.Name);

            builder
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.Description)
                .HasMaxLength(100)
                .IsRequired(false);
        }
    }
}
