using Domain.Core.Interfaces;

namespace Domain.User.Interfaces;

public interface IUserRepository: ICoreRepository
{
    Task AddUserAsync(Entities.User user);
    Task UpdateUserAsync(Entities.User user);
}