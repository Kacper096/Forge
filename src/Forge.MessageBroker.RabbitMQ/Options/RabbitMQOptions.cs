namespace Forge.MessageBroker.RabbitMQ.Options;

public class RabbitMQOptions : IRabbitMQOptions
{
    public const string DefaultSectionName = "rabbit";

    public string ConnectionName { get; set; } = string.Empty;
    public string HostName { get; set; } = string.Empty;
    public int Port { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = string.Empty;
}
