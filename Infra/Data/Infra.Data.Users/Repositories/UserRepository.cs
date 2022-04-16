using System.Linq.Expressions;
using Domain.User.Entities;
using Domain.User.Interfaces;
using Infra.Data.Core;
using Infra.Data.Users.Context;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IEnumerable<User>> GetUsers(Expression<Func<User, bool>> predicate)
    {
        var users = Context.Set<User>().Where(predicate);

        return await users.ToListAsync();
    }
}