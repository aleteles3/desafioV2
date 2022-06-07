using Domain.Core.Interfaces;
using Domain.Core.MemoryBus;
using Domain.Core.Security;
using Domain.Core.UserToken;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.IoC.Core;

public class DependencyInjector
{
    public static IServiceCollection AddServices(IServiceCollection services, IConfiguration configuration)
    {
        //Adding Core Injections
        services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUserToken, UserToken>();
        services.AddScoped<IMemoryBus, MemoryBus>();
        services.AddScoped<ISecurity, Security>();

        return services;
    }
}