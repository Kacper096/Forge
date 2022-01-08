namespace Forge.MessageBroker.RabbitMQ.Options
{
    public interface IRabbitMQOptions
    {
        string ConnectionName { get; set; }
        string HostName { get; set; }
        string Password { get; set; }
        int Port { get; set; }
        string UserName { get; set; }
        string VirtualHost { get; set; }
    }
}