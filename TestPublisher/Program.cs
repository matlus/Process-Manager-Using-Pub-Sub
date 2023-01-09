// See https://aka.ms/new-console-template for more information

using PostBindOrchestrator.Core;

internal static class Program
{
    private static async Task Main()
    {
        const string OrchestrationTask1TopicName = "pbo.orch.task1.topic";
        ////const string OrchestrationReplyTopicName = "pbo.orch.reply.topic";
        const string OrchestrationReplyQueueName = "pbo.orch.reply.que";

        var MessageBrokerConnectionString = "Endpoint=sb://matlussb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=CzSajVgYxHaJFLej1kz+CqRupsu10AeP2MU+HBxSv3A=;TransportType=AmqpWebSockets";

        using var publisher = new PublisherServiceBus();
        await publisher.Initialize(MessageBrokerConnectionString, OrchestrationTask1TopicName, OrchestrationReplyQueueName);

        var messageId = "1";
        var correlationId = Guid.NewGuid().ToString("N");

        var orchestrationTaskMessage = new OrchestrationTaskMessage(messageId, correlationId, "A1234567890", DateTimeOffset.UtcNow, OrchestrationTask.RevertToQuote);

        await publisher.Publish(orchestrationTaskMessage, messageId, correlationId);
    }
}