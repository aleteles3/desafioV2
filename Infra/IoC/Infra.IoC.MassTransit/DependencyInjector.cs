using Domain.MassTransit.Interfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.IoC.MassTransit;

public class DependencyInjector
{
    public static IServiceCollection AddServices(IServiceCollection services, Dictionary<string, 
        IEnumerable<Type>> queueConsumers)
    {
        services.AddMassTransit(x =>
        {
            services.AddScoped<IMassTransit, Domain.MassTransit.MassTransit>();
            
            x.AddHealthChecks();

            foreach (var consumer in queueConsumers.SelectMany(c => c.Value))
                x.AddConsumer(consumer);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                foreach (var queueConsumer in queueConsumers)
                {
                    cfg.ReceiveEndpoint(queueConsumer.Key, e =>
                    {
                        e.PrefetchCount = 10;
                        e.UseMessageRetry(r => r.Interval(2, 100));
                        foreach (var consumer in queueConsumer.Value)
                        {
                            e.ConfigureConsumer(context, consumer);
                        }
                    });
                }
            });
        });

        return services;
    }
}