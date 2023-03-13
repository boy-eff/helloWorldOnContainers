using System.Reflection;
using Achievements.Application.MassTransit.Consumers;
using MassTransit;

namespace Achievements.WebAPI.Extensions;

public static class MassTransitExtensions
{
    public static void ConfigureMassTransit(this IServiceCollection services, IConfiguration config)
    {
        services.AddMassTransit(x =>
        {
            var assembly = Assembly.GetAssembly(typeof(UserCreatedMessageConsumer));
            var host = config["RabbitMQ:Host"];
            var virtualHost = config["RabbitMQ:VirtualHost"];
            var username = config["RabbitMQ:Username"];
            var password = config["RabbitMQ:Password"];

            x.AddConsumers(assembly);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host, virtualHost, h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}