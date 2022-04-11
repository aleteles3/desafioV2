namespace Domain.Core.Interfaces;

public interface ICoreRepository
{
    Task BeginTransactionAsync();
    Task<int> SaveChangesAsync();
    Task CommitTransactionAsync();
    Task RollBackTransactionAsync();
}