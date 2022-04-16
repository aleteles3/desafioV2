using System.Linq.Expressions;
using Domain.Core.Interfaces;
using Domain.Product.Entities;

namespace Domain.Product.Interfaces
{
    public interface ICategoryRepository : ICoreRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync(Expression<Func<Category, bool>> predicate, 
            int? start = null, int? length = null, params Expression<Func<Category, object>>[] includes);
        Task<Category> GetCategoryByIdAsync(Guid id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task RemoveCategoryAsync(Category category);
    }
}