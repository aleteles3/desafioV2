using System.Linq.Expressions;
using Domain.Core.Interfaces;
using ProductDomain = Domain.Product.Entities.Product;

namespace Domain.Product.Interfaces
{
    public interface IProductRepository : ICoreRepository
    {
        Task<IEnumerable<ProductDomain>> GetProductsAsync(Expression<Func<ProductDomain, bool>> predicate, 
            int? start = null, int? length = null,
            params Expression<Func<ProductDomain, object>>[] includes);
        Task<ProductDomain> GetProductByIdAsync(Guid id);
        Task AddProductAsync(ProductDomain product);
        Task UpdateProductAsync(ProductDomain product);
        Task RemoveProductAsync(ProductDomain product);
    }
}