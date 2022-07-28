using Forge.Application.Messages;
using Forge.MessageBroker.RabbitMQ.Routing;

namespace Forge.WebHost.Bootstrap;

public static class MessageBrokerBootstrapper
{
    public static void ConfigurePublishMessages(IPublisherMessageDestinationProvider provider)
    {
        provider.Add<RabbitTestMessage>();
    }

    public static void ConfigureSubscribeMessages(IConsumerMessageDestinationProvider provider)
    {
        provider.Add<RabbitTestMessage>();

    }
}
