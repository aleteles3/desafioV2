using Domain.Product.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Product.EntitiesConfigurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("Ori_OrderItem", "Product");

        builder.HasKey(x => x.Id)
            .HasName("PK_Ori_OrderItem");

        builder.Property(x => x.Id)
            .HasColumnName("Ori_OrderItemId")
            .IsRequired();

        builder.Property(x => x.OrderId)
            .HasColumnName("Ord_OrderId")
            .IsRequired();

        builder.HasOne(x => x.Order)
            .WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.OrderId)
            .HasConstraintName("FK_Ori_OderItem_Ord_Order_Ord_OrderId");

        builder.Property(x => x.ProductId)
            .HasColumnName("Pro_ProductId")
            .IsRequired();

        builder.HasOne(x => x.Product)
            .WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.ProductId)
            .HasConstraintName("FK_Ori_OrderItem_Pro_Product_Pro_ProductId");

        builder.Property(x => x.ListPrice)
            .HasColumnName("Ori_ListPrice")
            .IsRequired();

        builder.Property(x => x.Discount)
            .HasColumnName("Ori_Discount")
            .IsRequired();
        
        builder.Ignore(x => x.ValidationResult);
        builder.Ignore(x => x.CascadeMode);
    }
}