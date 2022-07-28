namespace Forge.MessageBroker.RabbitMQ.Options
{
    public sealed record class DeadLetterExchangeOptions : IDeadLetterExchangeOptions
    {
        public bool Enabled { get; set; } = false;
        public string PrefixName { get; set; } = string.Empty;
    }
}
