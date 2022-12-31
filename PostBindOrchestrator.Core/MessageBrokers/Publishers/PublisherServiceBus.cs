using Azure.Messaging.ServiceBus;

namespace PostBindOrchestrator.Core;

public sealed class PublisherServiceBus : PublisherBase
{
    private bool disposed;
    private ServiceBusClient? serviceBusClient;
    private ServiceBusSender? serviceBusSender;

    private static readonly ServiceBusClientOptions serviceBusClientOptions = new()
    {
        TransportType = ServiceBusTransportType.AmqpWebSockets
    };

    protected override Task InitializeCore(string connectionString, string topicName, string queueName)
    {
        ConnectionString = connectionString;
        TopicName = topicName;

        serviceBusClient = new ServiceBusClient(connectionString, serviceBusClientOptions);
        serviceBusSender = serviceBusClient!.CreateSender(TopicName);

        return Task.CompletedTask;
    }

    protected override async Task PublishCore<T>(T message, string messageId, string correlationId)
    {
        var messageBody = SerializeMessage(message);
        var serviceBusMessage = InitializeMessageProperties(messageBody, messageId, correlationId, typeof(T));

        await serviceBusSender!.SendMessageAsync(serviceBusMessage);
    }

    protected override async Task Dispose(bool disposing)
    {
        if (disposing && !disposed)
        {
            await serviceBusClient!.DisposeAsync();

            disposed = true;
        }
    }

    private static ServiceBusMessage InitializeMessageProperties(byte[] messageBody, string messageId, string correlationId, Type messageType)
    {
        var serviceBusMessage = new ServiceBusMessage(messageBody)
        {
            MessageId = messageId,
            CorrelationId = correlationId,
            ContentType = "application/json"
        };

        serviceBusMessage.ApplicationProperties.Add("CreationDate", DateTimeOffset.UtcNow.ToString("o"));
        serviceBusMessage.ApplicationProperties.Add("MessageType", messageType.Name);

        return serviceBusMessage;
    }
}
