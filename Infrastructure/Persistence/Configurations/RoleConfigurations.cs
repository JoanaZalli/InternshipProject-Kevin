﻿using Domain.Entities;
using Infrastructure.Persistence.Seeds;
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
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .HasMany(x => x.Permissions)
                .WithMany(y => y.Roles)
                .UsingEntity<Role_Permission>();

            SeedData(builder);
        }

        private void SeedData(EntityTypeBuilder<Role> builder) {
            builder.HasData(new Role { Id = Guid.NewGuid(), Name = Roles.SuperAdmin });
            builder.HasData(new Role { Id = Guid.NewGuid(), Name = Roles.LoanOfficerBackOffice });
            builder.HasData(new Role { Id = Guid.NewGuid(), Name = Roles.LoanOfficerFrontOffice });
            builder.HasData(new Role { Id = Guid.NewGuid(), Name = Roles.Borrower });
            builder.HasData(new Role { Id = Guid.NewGuid(), Name = Roles.RegisteredUser });
        }
    }
}
