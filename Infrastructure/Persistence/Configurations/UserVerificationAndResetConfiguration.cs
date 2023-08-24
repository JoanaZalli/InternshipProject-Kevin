using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations {
    public class UserVerificationAndResetConfiguration : IEntityTypeConfiguration<UserVerificationAndReset> {
        public void Configure(EntityTypeBuilder<UserVerificationAndReset> builder) {
            builder.ToTable("UserVerificationAndReset")
                .HasKey(x => x.Id);

            builder
                .HasOne(x => x.User)
                .WithOne(x => x.UserVerificationAndReset)
                .HasForeignKey<UserVerificationAndReset>(x => x.UserEmail)
                .HasPrincipalKey<User>(x => x.Email)
                .IsRequired();
        }
    }
}
