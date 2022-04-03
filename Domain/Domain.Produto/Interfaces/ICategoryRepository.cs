using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Domain.Produto.Entities;

namespace DefaultNamespace
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync(Expression<Func<Category, bool>> predicate, 
            int? start, int? length, params Expression<Func<Category, object>>[] includes);
        Task<Category> GetCategoryByIdAsync(Guid id);
        Task<Guid> AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task RemoveCategoryAsync(Category category);
    }
}