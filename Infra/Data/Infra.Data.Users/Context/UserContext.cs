using Microsoft.EntityFrameworkCore;
using UserDomain = Domain.User.Entities.User;

namespace Infra.Data.Users.Context;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options){ }
    
    public DbSet<UserDomain> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserContext).Assembly);
    }
}