using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations {
    public class UserRoleConfiguration : IEntityTypeConfiguration<User_Role> {
        public void Configure(EntityTypeBuilder<User_Role> builder) {
            builder
                .ToTable("UserRoles")
                .HasKey(x => new { x.UserId, x.RoleId });
        }
    }
}
