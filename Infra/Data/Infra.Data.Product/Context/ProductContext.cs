using Domain.Product.Entities;
using Microsoft.EntityFrameworkCore;
using ProductDomain = Domain.Product.Entities.Product;

namespace Infra.Data.Product.Context
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductDomain> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductContext).Assembly);
        }
    }
}