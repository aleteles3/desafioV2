using System.Linq.Expressions;
using Domain.Core.Interfaces;

namespace Domain.User.Interfaces;

public interface IUserRepository: ICoreRepository
{
    Task AddUserAsync(Entities.User user);
    Task UpdateUserAsync(Entities.User user);
    Task<IEnumerable<Entities.User>> GetUsers(Expression<Func<Entities.User, bool>> predicate);
}