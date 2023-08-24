using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations {
    public class RoleConfigurations : IEntityTypeConfiguration<Role> {
        public void Configure(EntityTypeBuilder<Role> builder) {
            builder
                .ToTable("Roles");

            builder
                .HasKey(x => x.Id);

            builder
                .HasAlternateKey(x => x.Name);

            builder
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .HasMany(x => x.Permissions)
                .WithMany(y => y.Roles)
                .UsingEntity<Role_Permission>();
        }
    }
}
