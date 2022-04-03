using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Product.Interfaces;
using Infra.Data.Product.Context;
using Microsoft.EntityFrameworkCore;
using ProductDomain = Domain.Product.Entities.Product;

namespace Infra.Data.Product.Repositories
{
    public class ProductRepository : CoreRepository, IProductRepository
    {
        public ProductRepository(ProductContext productContext) : base(productContext) { }
        
        public async Task<IEnumerable<ProductDomain>> GetProductsAsync(Expression<Func<ProductDomain, bool>> predicate, 
            int? start, int? length, params Expression<Func<ProductDomain, object>>[] includes)
        {
            var query = Context.Set<ProductDomain>().Where(predicate);

            if (includes.Any())
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (start != null && length != null)
                query = query.Skip(start.Value).Take(length.Value);

            return await query.ToListAsync();
        }

        public async Task<ProductDomain> GetProductByIdAsync(Guid id)
        {
            return await Context.Set<ProductDomain>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Guid> AddProductAsync(ProductDomain product)
        {
            await Context.AddAsync(product);
            await SaveChangesAsync();
            return product.Id;
        }

        public async Task UpdateProductAsync(ProductDomain product)
        {
            Context.Update(product);
            await SaveChangesAsync();
        }

        public async Task RemoveProductAsync(ProductDomain product)
        {
            Context.Remove(product);
            await SaveChangesAsync();
        }
    }
}