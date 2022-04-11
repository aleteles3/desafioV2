using Domain.Core.Entities;
using Domain.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Core
{
    public abstract class CoreRepository : ICoreRepository
    {
        protected DbContext Context;

        protected CoreRepository(DbContext context)
        {
            Context = context;
        }
        
        public async Task BeginTransactionAsync()
        {
            if (Context.Database.CurrentTransaction != null)
                return;
            await Context.Database.BeginTransactionAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await Context.Database.CommitTransactionAsync();
        }

        public async Task RollBackTransactionAsync()
        {
            await Context.Database.RollbackTransactionAsync();
        }

        protected async Task AddAsync<T>(Entity<T> entity)
        {
            entity.SetDateInc(DateTimeOffset.UtcNow);
            entity.SetDateAlter(DateTimeOffset.UtcNow);

            await Context.AddAsync(entity);
        }

        protected void Update<T>(Entity<T> entity)
        {
            entity.SetDateAlter(DateTimeOffset.UtcNow);
            Context.Update(entity);
        }
    }
}