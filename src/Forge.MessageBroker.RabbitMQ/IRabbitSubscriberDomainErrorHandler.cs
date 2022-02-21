namespace Forge.MessageBroker.RabbitMQ
{
    public interface IRabbitSubscriberDomainErrorHandler
    {
        Task HandleAsync(Exception exception);
    }
}
