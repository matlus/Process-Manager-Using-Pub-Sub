// See https://aka.ms/new-console-template for more information

using PostBindOrchestrator.Core;

const string OrchestrationReplyTopicName = "pbo.orch.reply.topic";
const string OrchestrationReplyQueueName = "pbo.orch.reply.que";

var MessageBrokerConnectionString = "Endpoint=sb://matlussb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=CzSajVgYxHaJFLej1kz+CqRupsu10AeP2MU+HBxSv3A=;TransportType=AmqpWebSockets";

Console.WriteLine("Hello, World!");
using var publisher = new PublisherServiceBus();
await publisher.Initialize(MessageBrokerConnectionString, OrchestrationReplyTopicName, OrchestrationReplyQueueName);

var messageId = "1";
var correlationId = Guid.NewGuid().ToString("N");

var orchestrationTaskMessage = new OrchestrationTaskMessage(messageId, correlationId, "A1234567890", DateTimeOffset.UtcNow, OrchestrationTask.RevertToQuote);

await publisher.Publish(orchestrationTaskMessage, messageId, correlationId);
