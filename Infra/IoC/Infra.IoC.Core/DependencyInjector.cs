using Domain.Core.Interfaces;
using Domain.Core.MemoryBus;
using Domain.Core.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.IoC.Core;

public class DependencyInjector
{
    public static IServiceCollection AddServices(IServiceCollection services, IConfiguration configuration)
    {
        //Adding Core Injections
        services.AddScoped<IMemoryBus, MemoryBus>();
        services.AddScoped<ISecurity, Security>();

        return services;
    }
}