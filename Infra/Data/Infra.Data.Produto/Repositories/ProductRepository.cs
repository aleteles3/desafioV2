using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DefaultNamespace;
using Domain.Domain.Produto.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Infra.Data.Produto.Repositories
{
    public class ProductRepository : CoreRepository, IProductRepository
    {
        public ProductRepository(DbContext productContext) : base(productContext) { }
        
        public async Task<IEnumerable<Product>> GetProductsAsync(Expression<Func<Product, bool>> predicate, int? start, 
            int? length, params Expression<Func<Product, object>>[] includes)
        {
            var query = Context.Set<Product>().Where(predicate);

            if (includes.Any())
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (start != null && length != null)
                query = query.Skip(start.Value).Take(length.Value);

            return await query.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await Context.Set<Product>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Guid> AddProductAsync(Product product)
        {
            await Context.AddAsync(product);
            await SaveChangesAsync();
            return product.Id;
        }

        public async Task UpdateProductAsync(Product product)
        {
            Context.Update(product);
            await SaveChangesAsync();
        }

        public async Task RemoveProductAsync(Product product)
        {
            Context.Remove(product);
            await SaveChangesAsync();
        }
    }
}