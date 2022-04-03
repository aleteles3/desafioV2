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
    public class CategoryRepository : CoreRepository, ICategoryRepository
    {
        public CategoryRepository(DbContext productContext) : base(productContext) { }
        
        public async Task<IEnumerable<Category>> GetCategoriesAsync(Expression<Func<Category, bool>> predicate, 
            int? start, int? length, params Expression<Func<Category, object>>[] includes)
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

        public async Task<Guid> AddCategoryAsync(Category category)
        {
            await Context.AddAsync(category);
            await SaveChangesAsync();
            return category.Id;
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            Context.Update(category);
            await SaveChangesAsync();
        }

        public async Task RemoveCategoryAsync(Category category)
        {
            Context.Remove(category);
            await SaveChangesAsync();
        }
    }
}