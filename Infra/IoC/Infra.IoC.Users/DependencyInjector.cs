using Application.User.AutoMapper;
using Application.Users.Interfaces;
using Domain.User.Cqrs.User.Handlers;
using Domain.User.Interfaces;
using Infra.Data.Users.Context;
using Infra.Data.Users.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserAppService = Application.Users.Services.UserAppService;

namespace Infra.IoC.Users;

public static class DependencyInjector
{
    public static IServiceCollection AddServices(IServiceCollection services, IConfiguration configuration)
    {
        //Adding Database Connection
        services.AddDbContext<UserContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection") ?? string.Empty,
                x => x.MigrationsAssembly(typeof(UserContext).Assembly.FullName)));
        
        //Adding AutoMapper
        services.AddAutoMapper(typeof(AutoMapperConfiguration));
        
        //Adding Mediator
        services.AddMediatR(new[]
        {
            typeof(UserCommandHandler)
        });
        
        //Adding Services
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserAppService, UserAppService>();

        return services;
    }
}