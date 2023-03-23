using System.Reflection;
using Achievements.Application.MassTransit.Consumers;
using MassTransit;
using Shared.MassTransit.Filters;
using Shared.Messages;

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
                
                cfg.UseConsumeFilter(typeof(ConsumeLoggingFilter<>), context);

                cfg.ReceiveEndpoint(config["RabbitMQ:ReceiveEndpoints:UserCreated"], x =>
                {
                    x.Bind<UserCreatedMessage>();
                    x.ConfigureConsumer<UserCreatedMessageConsumer>(context);
                });
                
                cfg.ReceiveEndpoint(config["RabbitMQ:ReceiveEndpoints:WordAddedToDictionary"], x =>
                {
                    x.Bind<WordAddedToDictionaryMessage>();
                    x.ConfigureConsumer<WordAddedToDictionaryMessageConsumer>(context);
                });
                
                cfg.ReceiveEndpoint(config["RabbitMQ:ReceiveEndpoints:WordCollectionCreated"], x =>
                {
                    x.Bind<WordCollectionCreatedMessage>();
                    x.ConfigureConsumer<WordCollectionCreatedMessageConsumer>(context);
                });
                
                cfg.ReceiveEndpoint(config["RabbitMQ:ReceiveEndpoints:WordCollectionTestPassed"], x =>
                {
                    x.Bind<WordCollectionTestPassedMessage>();
                    x.ConfigureConsumer<WordCollectionTestPassedMessageConsumer>(context);
                });
            });
        });
    }
}