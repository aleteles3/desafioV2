using Domain.User.Interfaces;
using Infra.Data.Core;
using Infra.Data.Users.Context;

namespace Infra.Data.Users.Repositories;

public class UserRepository : CoreRepository, IUserRepository
{
    public UserRepository(UserContext context) : base(context) { }

    public async Task AddUserAsync(Domain.User.Entities.User user)
    {
        await AddAsync(user);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(Domain.User.Entities.User user)
    {
        Update(user);
        await Context.SaveChangesAsync();
    }
}