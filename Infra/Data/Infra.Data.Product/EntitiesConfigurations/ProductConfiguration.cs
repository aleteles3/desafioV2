using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductDomain = Domain.Product.Entities.Product;

namespace Infra.Data.Product.EntitiesConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductDomain>
    {
        public void Configure(EntityTypeBuilder<ProductDomain> builder)
        {
            builder.ToTable("Pro_Product", "Product");

            builder.HasKey(x => x.Id)
                .HasName("PK_Pro_Product");

            builder.Property(x => x.Id)
                .HasColumnName("Pro_ProductId")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("Pro_Name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("Pro_Description")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("Pro_Price")
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(x => x.Stock)
                .HasColumnName("Pro_Stock")
                .IsRequired();

            builder.Property(x => x.CategoryId)
                .HasColumnName("Cat_CategoryId")
                .IsRequired();

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .HasConstraintName("FK_Pro_Product_Cat_Category_Cat_CategoryId");
        }
    }
}