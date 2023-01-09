namespace PostBindOrchestrator.Core;

public abstract class PublisherBase : IDisposable
{
    protected string? ConnectionString { get; set; }
    protected string? TopicName { get; set; }

    public async Task Initialize(string connectionString, string topicName, string queueName) => await InitializeCore(connectionString, topicName, queueName);

    public async Task Publish<T>(T message, string messageId, string correlationId)
    {
        try
        {
            await PublishCore(message, messageId, correlationId);
        }
        catch (Exception ex)
        {
            throw new MessageBrokerPublishException($"Failed to publish a message to the {TopicName} topic, with connection string {ConnectionString}. The Original Exception Type is: {ex.GetType().Name} and the Original Exception Message is: {ex.Message}", ex);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected static byte[] SerializeMessage<T>(T message) => ApplicationSerializer.Serialize(message);

    protected abstract Task InitializeCore(string connectionString, string topicName, string queueName);

    protected abstract Task PublishCore<T>(T message, string messageId, string correlationId);

    protected abstract Task Dispose(bool disposing);
}
