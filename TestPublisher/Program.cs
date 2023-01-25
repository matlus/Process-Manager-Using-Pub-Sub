// See https://aka.ms/new-console-template for more information

using PostBindOrchestrator.Core;

internal static class Program
{
    private static async Task Main()
    {
        ////const string OrchestrationTaskTopicName = "pbo.orch.sendcoidocument.topic";
        const string OrchestrationReplyTopicName = "pbo.orch.reply.topic";
        const string OrchestrationReplyQueueName = "pbo.orch.reply.que";

        var MessageBrokerConnectionString = "Endpoint=sb://matlussb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=CzSajVgYxHaJFLej1kz+CqRupsu10AeP2MU+HBxSv3A=;TransportType=AmqpWebSockets";

        using var publisher = new PublisherServiceBus();
        await publisher.Initialize(MessageBrokerConnectionString, OrchestrationReplyQueueName, OrchestrationReplyQueueName);

        var messageId = "1";
        var correlationId = Guid.NewGuid().ToString("N");

        var orchestrationTaskMessage = new OrchestrationReplyMessage(messageId, correlationId, "A1234567890", DateTimeOffset.UtcNow, OrchestrationTask.AssociateClientWithPolicy, "Finished");

        await publisher.Publish(orchestrationTaskMessage, messageId, correlationId);
    }
}