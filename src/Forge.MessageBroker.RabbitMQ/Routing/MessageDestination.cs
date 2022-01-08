namespace Forge.MessageBroker.RabbitMQ.Routing
{
    internal record class MessageDestination : IMessageDestination
    {
        public string RoutingKey { get; }
        public string Exchange { get; }
        public string Queue { get; }

        public MessageDestination(string routingKey, string exchange, string queue)
        {
            RoutingKey = routingKey;
            Exchange = exchange;
            Queue = queue;
        }
    }
}
