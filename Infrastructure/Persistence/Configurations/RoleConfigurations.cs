﻿using Azure;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence.Configurations {
    public class RoleConfigurations : IEntityTypeConfiguration<Role> {
        public void Configure(EntityTypeBuilder<Role> builder) {
            builder.ToTable("Roles");

            builder.HasKey(x => x.RoleId);

            builder
                .HasMany(x => x.Permissions)
                .WithMany(y => y.Roles)
                .UsingEntity<Role_Permission>();

            
        }
    }
}
