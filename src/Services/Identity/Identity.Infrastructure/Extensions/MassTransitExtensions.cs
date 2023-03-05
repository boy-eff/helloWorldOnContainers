using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.Extensions;

public static class MassTransitExtensions
{
    public static void ConfigureMassTransit(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddMassTransit(x =>
        {
            var assembly = Assembly.GetEntryAssembly();
            var host = config["RabbitMQ:Host"];
            var virtualHost = config["RabbitMQ:VirtualHost"];
            var username = config["RabbitMQ:Username"];
            var password = config["RabbitMQ:Password"];

            x.AddConsumers(assembly);

            x.UsingRabbitMq((context, cfg) => {
                cfg.Host(host, virtualHost, h => {
                    h.Username(username);
                    h.Password(password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}