using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserDomain = Domain.User.Entities.User;

namespace Infra.Data.Users.EntitiesConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<UserDomain>
{
    public void Configure(EntityTypeBuilder<UserDomain> builder)
    {
        builder.ToTable("Use_User", "User");

        builder.HasKey(x => x.Id)
            .HasName("PK_Use_User");

        builder.Property(x => x.Id)
            .HasColumnName("Use_UserId")
            .IsRequired();

        builder.Property(x => x.Login)
            .HasColumnName("Use_Login")
            .IsRequired();

        builder.HasIndex(x => x.Login)
            .HasDatabaseName("IX_Use_Login");

        builder.Property(x => x.Password)
            .HasColumnName("Use_Password")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("Use_Status")
            .IsRequired();

        builder.Ignore(x => x.ValidationResult);
        builder.Ignore(x => x.CascadeMode);
    }
}