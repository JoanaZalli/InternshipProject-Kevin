using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations {
    public class RolePermissionConfiguration : IEntityTypeConfiguration<Role_Permission> {
        public void Configure(EntityTypeBuilder<Role_Permission> builder) {
            builder
                .ToTable("RolePermissions")
                .HasKey(x => new { x.RoleId, x.PermissionId });
        }
    }
}
