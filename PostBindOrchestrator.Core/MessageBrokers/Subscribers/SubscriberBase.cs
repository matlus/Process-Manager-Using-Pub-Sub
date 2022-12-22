namespace PostBindOrchestrator.Core;

public abstract class SubscriberBase : IDisposable
{
    protected string? TopicName;
    protected string? QueueName;

    public Task Initialize(string connectionString, string topicName, string queueName)
    {
        return InitializeCore(connectionString, topicName, queueName);
    }

    public Task Subscribe(Func<SubscriberBase, MessageReceivedEventArgs, Task> receiveCallback)
    {
        return SubscribeCore(receiveCallback);
    }

    public Task Acknowledge(string acknowledgetoken)
    {
        return AcknowledgeCore(acknowledgetoken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected abstract Task InitializeCore(string connectionString, string topicName, string queueName);

    protected abstract Task SubscribeCore(Func<SubscriberBase, MessageReceivedEventArgs, Task> receiveCallback);

    protected abstract Task AcknowledgeCore(string acknowledgetoken);

    protected abstract Task Dispose(bool disposing);
}
