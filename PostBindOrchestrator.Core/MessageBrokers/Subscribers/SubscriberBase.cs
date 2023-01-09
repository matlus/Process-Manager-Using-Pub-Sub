namespace PostBindOrchestrator.Core;

public abstract class SubscriberBase : IDisposable
{
    protected string? TopicName { get; set; }
    protected string? QueueName { get; set; }

    public Task Initialize(string connectionString, string topicName, string queueName, CancellationToken cancellationToken) => InitializeCore(connectionString, topicName, queueName, cancellationToken);

    public Task Subscribe(Func<SubscriberBase, MessageReceivedEventArgs, Task> receiveCallback, CancellationToken cancellationToken)
    {
        return SubscribeCore(receiveCallback, cancellationToken);
    }

    public Task Acknowledge(object acknowledgetoken, CancellationToken cancellationToken)
    {
        return AcknowledgeCore(acknowledgetoken, cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected abstract Task InitializeCore(string connectionString, string topicName, string queueName, CancellationToken cancellationToken);

    protected abstract Task SubscribeCore(Func<SubscriberBase, MessageReceivedEventArgs, Task> receiveCallback, CancellationToken cancellationToken);

    protected abstract Task AcknowledgeCore(object acknowledgetoken, CancellationToken cancellationToken);

    protected abstract Task Dispose(bool disposing);
}
