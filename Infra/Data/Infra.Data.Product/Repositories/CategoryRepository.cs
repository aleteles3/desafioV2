using System.Linq.Expressions;
using Domain.Product.Entities;
using Domain.Product.Interfaces;
using Infra.Data.Core;
using Infra.Data.Product.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Product.Repositories
{
    public class CategoryRepository : CoreRepository, ICategoryRepository
    {
        public CategoryRepository(ProductContext productContext) : base(productContext) { }
        
        public async Task<IEnumerable<Category>> GetCategoriesAsync(Expression<Func<Category, bool>> predicate, 
            int? start = null, int? length = null, params Expression<Func<Category, object>>[] includes)
        {
            var query = Context.Set<Category>().Where(predicate);

            if (includes.Any())
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (start != null && length != null)
                query = query.Skip(start.Value).Take(length.Value);

            return await query.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            return await Context.Set<Category>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await AddAsync(category);
            await SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            Update(category);
            await SaveChangesAsync();
        }

        public async Task RemoveCategoryAsync(Category category)
        {
            Context.Remove(category);
            await SaveChangesAsync();
        }
    }
}