using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Infra.Data.Produto.Repositories
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

        public virtual async Task<int> SaveChangesAsync()
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
    }
}