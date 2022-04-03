using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Domain.Produto.Entities;

namespace DefaultNamespace
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(Expression<Func<Product, bool>> predicate, int? start, int? length,
            params Expression<Func<Product, object>>[] includes);
        Task<Product> GetProductByIdAsync(Guid id);
        Task<Guid> AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task RemoveProductAsync(Product product);
    }
}