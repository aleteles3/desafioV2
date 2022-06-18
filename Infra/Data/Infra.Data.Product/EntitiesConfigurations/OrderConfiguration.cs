using Domain.Product.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Product.EntitiesConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Ord_Order", "Product");

        builder.HasKey(x => x.Id)
            .HasName("PK_Ord_Order");

        builder.Property(x => x.Id)
            .HasColumnName("Ord_OrderId")
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("Use_UserId")
            .IsRequired();

        builder.Property(x => x.OrderStatus)
            .HasColumnName("Ord_OrderStatus")
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("FK_Ord_Order_Use_User_Use_UserId");

        builder.HasMany(x => x.OrderItems)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId);
        
        builder.Ignore(x => x.ValidationResult);
        builder.Ignore(x => x.CascadeMode);
    }
}