using Forge.MessageBroker.RabbitMQ.Arrangement;
using Forge.MessageBroker.RabbitMQ.Connections;
using Forge.MessageBroker.RabbitMQ.Exchange;
using Forge.MessageBroker.RabbitMQ.Options;
using Forge.MessageBroker.RabbitMQ.Routing;
using Forge.MessageBroker.RabbitMQ.Routing.Client;
using Forge.MessageBroker.RabbitMQ.Routing.Subscribers;
using Forge.MessageBroker.RabbitMQ.Serializers;
using Forge.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Forge.MessageBroker.RabbitMQ
{
    public static class ServiceCollectionExtensions
    {
        private const string SuffixPublishConnection = "publish";
        private const string SuffixSubscribeConnection = "subscribe";

        public static IServiceCollection AddRabbitMQ(this IServiceCollection services,
                                                     IConfiguration configuration,
                                                     IExchangeOptionsInitializer exchangeOptions = null,
                                                     params Assembly[] assemblies)
        {
            exchangeOptions = ExchangeOptionsInitializerFactory.CreateInstance(exchangeOptions);
            var rabbitOptions = configuration.GetOptions<RabbitMQOptions>(RabbitMQOptions.DefaultSectionName);
            services.AddSingleton<IRabbitMQOptions>(rabbitOptions);
            services.AddSingleton<IDeadLetterExchangeOptions, DeadLetterExchangeOptions>()
                    .AddSingleton<IExchangeOptionsInitializer>(exchangeOptions);
            services.AddSingleton<IExchangeInitializer, ExchangeInitializer>()
                    .AddSingleton<IArrangementBuilder, ArrangementBuilder>()
                    .AddSingleton<IClientMessageDestinations, ClientMessageDestinations>()
                    .AddSingleton<ISubscriberMessageDestinations, SubscriberMessageDestinations>()
                    .AddSingleton<IRabbitMessageSerializer, JsonRabbitMessageSerializer>()
                    .AddSingleton<IRabbitMqClient, RabbitMqClient>();

            var connectionFactory = new RabbitMqConnectionFactory(rabbitOptions);
            var publishConnection = connectionFactory.Generate(SuffixPublishConnection);
            var subscribeConnection = connectionFactory.Generate(SuffixSubscribeConnection);

            services.AddSingleton<IPublishConnection>(new PublishConnection(publishConnection))
                    .AddSingleton<ISubscribeConnection>(new SubscribeConnection(subscribeConnection));

            services.AddHostedService<RabbitMqSubscribersService>();

            RegisterHandlers(services, assemblies);
            return services;
        }

        public static IApplicationBuilder UseRabbitMq(this IApplicationBuilder builder,
                                                      Action<IConsumerMessageDestinationProvider> subscriberMessageDestinations = null,
                                                      Action<IPublisherMessageDestinationProvider> clientMessageDestinations = null)
        {
            var serviceProvider = builder.ApplicationServices;
            var subscriberMessageDestinationsService = serviceProvider.GetRequiredService<ISubscriberMessageDestinations>();
            var clientMessageDestinationsService = serviceProvider.GetRequiredService<IClientMessageDestinations>();

            var arrangementBuilder = serviceProvider.GetRequiredService<IArrangementBuilder>();
            var messageDestinationProvider = new MessageDestinationProvider(arrangementBuilder);

            subscriberMessageDestinations?.Invoke(messageDestinationProvider);
            clientMessageDestinations?.Invoke(messageDestinationProvider);
            messageDestinationProvider.ApplyDestinations(subscriberMessageDestinationsService, clientMessageDestinationsService);

            messageDestinationProvider.Dispose();
            return builder;
        }

        private static void RegisterHandlers(IServiceCollection services, Assembly[] assemblies)
        {
            var handleInterface = typeof(IHandle<>);
            var genericTypes = AssemblyUtil.FindGenericDerivedTypes(assemblies, handleInterface).ToList();
            genericTypes.ForEach(type =>
            {
                var baseType = type.GetInterfaces().First(i => i.GetGenericTypeDefinition() == handleInterface);
                services.AddSingleton(baseType, type);
            });
        }
    }
}
