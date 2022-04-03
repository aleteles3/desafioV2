using Domain.Product.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Product.EntitiesConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Cat_Category", "Product");

            builder.HasKey(x => x.Id)
                .HasName("PK_Cat_Category");
            
            builder.Property(x => x.Id)
                .HasColumnName("Cat_CategoryId")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("Cat_Name")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasMany(x => x.Products)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}