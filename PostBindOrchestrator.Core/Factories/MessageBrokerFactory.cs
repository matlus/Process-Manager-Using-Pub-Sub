namespace PostBindOrchestrator.Core;

public static class MessageBrokerFactory
{
    public static PublisherBase CreateMessageBrokerPublisher(MessageBrokerType messageBrokerType) => messageBrokerType switch
    {
        MessageBrokerType.ServiceBus => new PublisherServiceBus(),
        MessageBrokerType.RabbitMq => new PublisherRabbitMq(),
        _ => throw new MessageBrokerTypeNotSupportedException($"The MessageBrokerType: \"{messageBrokerType}\" for Message Broker Subscribers, is not supported yet"),
    };

    public static SubscriberBase CreateMessageBrokerSubscriber(MessageBrokerType messageBrokerType) => messageBrokerType switch
    {
        MessageBrokerType.ServiceBus => new SubscriberServiceBus(),
        MessageBrokerType.RabbitMq => new SubscriberRabbitMq(),
        _ => throw new MessageBrokerTypeNotSupportedException($"The MessageBrokerType: \"{messageBrokerType}\" for Message Broker Subscribers, is not supported yet"),
    };
}
