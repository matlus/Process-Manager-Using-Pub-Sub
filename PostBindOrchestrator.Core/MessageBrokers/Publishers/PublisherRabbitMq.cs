using RabbitMQ.Client;

namespace PostBindOrchestrator.Core;
public sealed class PublisherRabbitMq : PublisherBase
{
    private bool disposed;
    private IConnection connection = default!;
    private IModel channel = default!;
    private string topicName = default!;

    protected override Task InitializeCore(string connectionString, string topicName, string queueName)
    {
        var connectionFactory = new ConnectionFactory()
        {
            Uri = new Uri(connectionString),
        };

        ConnectionString = connectionString;
        TopicName = topicName;

        connection = connectionFactory.CreateConnection();
        channel = connection.CreateModel();
        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        this.topicName = topicName;

        return Task.CompletedTask;
    }

    protected override Task PublishCore<T>(T message, string messageId, string correlationId)
    {
        var basicProperties = InitializeBasicProperties(channel.CreateBasicProperties(), messageId, correlationId, typeof(T));

        var messageBody = SerializeMessage(message);

        channel.BasicPublish(topicName, routingKey: string.Empty, basicProperties, messageBody);

        return Task.CompletedTask;
    }

    protected override Task Dispose(bool disposing)
    {
        if (disposing && !disposed)
        {
            channel.Close();
            channel.Dispose();

            connection.Close();
            connection.Dispose();

            disposed = true;
        }

        return Task.CompletedTask;
    }

    private static IBasicProperties InitializeBasicProperties(IBasicProperties basicProperties, string messageId, string correlationId, Type messageType)
    {
        basicProperties.Persistent = true;
        basicProperties.MessageId = messageId;
        basicProperties.CorrelationId = correlationId;
        basicProperties.ContentType = "application/json";
        basicProperties.Type = messageType.Name;

        var propertiesDictionary = new Dictionary<string, object>();
        basicProperties.Headers = propertiesDictionary;
        propertiesDictionary.Add("creation_date", DateTimeOffset.UtcNow.ToString("o"));

        return basicProperties;
    }
}
