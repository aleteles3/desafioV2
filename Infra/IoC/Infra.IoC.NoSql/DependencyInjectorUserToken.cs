using Domain.NoSql;
using Domain.NoSql.UserToken.Interfaces;
using Infra.Data.NoSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.IoC.NoSql;

public class DependencyInjectorUserToken
{
    public static IServiceCollection AddServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        //Adding Mongo Database Connection
        serviceCollection.AddScoped<IUserTokenRepository, UserTokenRepository>(sp =>
        {
            var mongoDbSettings = new MongoDbSettings(
                connectionUri: configuration["MongoDb:ConnectionString"],
                database: configuration["MongoDb:Database"]);

            return new UserTokenRepository(mongoDbSettings);
        });

        return serviceCollection;
    }
}