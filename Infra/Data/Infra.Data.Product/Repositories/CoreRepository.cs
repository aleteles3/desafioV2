using Domain.Product.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Product.Repositories
{
    public abstract class CoreRepository
    {
        protected DbContext Context;

        protected CoreRepository(DbContext context)
        {
            Context = context;
        }
        
        public virtual async Task BeginTransactionAsync()
        {
            if (Context.Database.CurrentTransaction != null)
                return;
            await Context.Database.BeginTransactionAsync();
        }

        protected virtual async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public virtual async Task CommitTransactionAsync()
        {
            await Context.Database.CommitTransactionAsync();
        }

        public virtual async Task RollBackTransactionAsync()
        {
            await Context.Database.RollbackTransactionAsync();
        }

        protected async Task AddAsync<T>(Entity<T> entity)
        {
            entity.SetDateInc(DateTimeOffset.Now);
            entity.SetDateAlter(DateTimeOffset.Now);

            await Context.AddAsync(entity);
        }

        protected void Update<T>(Entity<T> entity)
        {
            entity.SetDateAlter(DateTimeOffset.Now);
            Context.Update(entity);
        }
    }
}