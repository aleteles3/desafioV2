using Domain.Product.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Product.EntitiesConfigurations.Shared;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
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

        builder.HasMany(x => x.Orders)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder.Ignore(x => x.ValidationResult);
        builder.Ignore(x => x.CascadeMode);
    }
}