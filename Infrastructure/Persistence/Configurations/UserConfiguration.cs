using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations {
    public class UserConfiguration : IEntityTypeConfiguration<User> {
        public void Configure(EntityTypeBuilder<User> builder) {
            builder
                .ToTable("Users")
                .HasKey(x => x.Id);

            builder
                .HasAlternateKey(x => x.Username);

            builder
                .HasAlternateKey(x => x.Email);

            builder
                .Property(x => x.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.Username)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.Email)
                .HasMaxLength(150)
                .IsRequired();

            builder
                .Property(x => x.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .Property(x => x.Prefix)
                .HasMaxLength(5)
                .IsRequired();

            builder
                .Property(x => x.PasswordHash)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(x => x.PasswordSalt)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .HasMany(x => x.Roles)
                .WithMany(y => y.Users)
                .UsingEntity<User_Role>();

            builder
                .HasMany(x => x.Borrowers)
                .WithOne(y => y.User)
                .IsRequired();

            builder
                .HasOne(x => x.UserVerificationAndReset)
                .WithOne(x => x.User);
        }
    }
}
