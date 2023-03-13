using System.Reflection;
using MassTransit;
using Shared.Messages;
using Words.BusinessAccess.MassTransit.Consumers;
using Words.BusinessAccess.MassTransit.Filters;
using Words.DataAccess;

namespace Words.WebAPI.Extensions;

public static class MassTransitExtensions
{
    public static void ConfigureMassTransit(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddMassTransit(x =>
        {
            var assembly = Assembly.GetAssembly(typeof(UserCreatedMessageConsumer));
            var host = config["RabbitMQ:Host"];
            var virtualHost = config["RabbitMQ:VirtualHost"];
            var username = config["RabbitMQ:Username"];
            var password = config["RabbitMQ:Password"];
            
            x.AddEntityFrameworkOutbox<WordsDbContext>(o =>
            {
                o.UseSqlServer();
                
                o.QueryDelay = TimeSpan.FromSeconds(1);
                
                o.UseBusOutbox();
            });
            
            x.AddConsumers(assembly);

            x.UsingRabbitMq((context, cfg) => {
                cfg.Host(host, virtualHost, h => {
                    h.Username(username);
                    h.Password(password);
                });

                cfg.UseConsumeFilter(typeof(LoggingFilter<>), context); 
                
                cfg.ReceiveEndpoint(config["RabbitMQ:ReceiveEndpoints:UserCreated"], x =>
                {
                    x.Bind<UserCreatedMessage>();
                    x.ConfigureConsumer<UserCreatedMessageConsumer>(context);
                });
            });
        });
    }
}