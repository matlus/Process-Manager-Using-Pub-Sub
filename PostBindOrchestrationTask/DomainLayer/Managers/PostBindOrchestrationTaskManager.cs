using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Amqp.Framing;
using PostBindOrchestrator.Core;
using PostBindOrchestrator.Core.Models.OrchestrationMessages;

namespace PostBindOrchestrationTask.DomainLayer;

internal sealed class PostBindOrchestrationTaskManager : IDisposable
{
    private const string OrchestrationReplyTopicName = "pbo.orch.reply1.topic";
    private const string OrchestrationReplyQueueName = "pbo.orch.reply.que";
    private const string OrchestrationExceptionTopicName = "pbo.orch.exception.topic";
    private const string OrchestrationExceptionQueueName = "pbo.orch.exception.que";

    private readonly ServiceLocatorBase serviceLocator;
    private ConfigurationProvider? configurationProvider;
    private ApplicationLogger? applicationLogger;
    private PublisherBase? publisherOrchestrationTaskReply;
    private PublisherBase? publisherOrchestrationTaskEception;
    private bool publishersHaveBeenInitialized;
    private bool disposed;

    private ConfigurationProvider ConfigurationProvider => configurationProvider ??= serviceLocator.CreateConfigurationProvider();

    private ApplicationLogger ApplicationLogger => applicationLogger ??= new ApplicationLogger(serviceLocator.CreateLogger());

    public PostBindOrchestrationTaskManager(ServiceLocatorBase serviceLocator) => this.serviceLocator = serviceLocator;

    private async Task InitializePublishers()
    {
        if (!publishersHaveBeenInitialized)
        {
            var messageBrokerSettings = ConfigurationProvider.GetMessageBrokerSettings();

            publisherOrchestrationTaskReply = serviceLocator.CreateMessageBrokerPublisher();
            await publisherOrchestrationTaskReply.Initialize(messageBrokerSettings.MessageBrokerConnectionString, OrchestrationReplyTopicName, OrchestrationReplyQueueName);

            publisherOrchestrationTaskEception = serviceLocator.CreateMessageBrokerPublisher();
            await publisherOrchestrationTaskEception.Initialize(messageBrokerSettings.MessageBrokerConnectionString, OrchestrationExceptionTopicName, OrchestrationExceptionQueueName);

            publishersHaveBeenInitialized = true;
        }
    }

    public async Task StartTask(ServiceBusReceivedMessage serviceBusReceivedMessage, CancellationToken cancellationToken)
    {
        var logEvent = LogEvent.StartTask;
        var orchestrationTaskMessage = GetOrchestrationTaskMessage(serviceBusReceivedMessage.Body);
        logEvent = LogEvent.OrchestrationTaskMessageReceived;

        try
        {
            await InitializePublishers();
            logEvent = LogEvent.OrchestrationTaskStarted;
            // Create Orchestration Record in Db using orchestrationMessageTask
            logEvent = LogEvent.OrchestrationTaskStartRecorded;
            // Do the Task
            logEvent = LogEvent.OrchestrationTaskFinished;
            // Record Task Finished in Db
            logEvent = LogEvent.OrchestrationTaskFinishedRecorded;
            // Publish Reply meesage
            var messageId = Guid.NewGuid().ToString("N");
            await publisherOrchestrationTaskReply!.Publish(new OrchestrationReplyMessage(
                messageId,
                orchestrationTaskMessage.CorrelationId,
                orchestrationTaskMessage.PolicyNumber,
                DateTimeOffset.UtcNow,
                OrchestrationTask.SendCoIDocument,
                "Completed"), messageId, orchestrationTaskMessage.CorrelationId);
            logEvent = LogEvent.OrchestrationTaskReplyPublished;
            ////throw new MessageBrokerPublishException($"There was an exception Publising a message to the topic: {OrchestrationReplyTopicName}");
        }
        catch (Exception e)        
        {
            ApplicationLogger.LogError(logEvent, e, $"{nameof(StartTask)}:catch block", orchestrationTaskMessage);

            var messageId = Guid.NewGuid().ToString("N");
            var orchestrationExceptionMessage = new OrchestrationExceptionMessage(
                messageId,
                orchestrationTaskMessage.CorrelationId,
                orchestrationTaskMessage.PolicyNumber,
                DateTimeOffset.UtcNow,
                OrchestrationTask.SendCoIDocument,
                logEvent,
                (ExceptionData)e);

            // TODO: Record Exception to Db
            //// await DataFacade.RecordTaskException(orchestrationExceptionMessage);            
            await publisherOrchestrationTaskEception!.Publish(orchestrationExceptionMessage, messageId, orchestrationTaskMessage.CorrelationId);
        }
    }

    private static OrchestrationTaskMessage GetOrchestrationTaskMessage(BinaryData binaryData)
    {
        return ApplicationSerializer.Deserialize<OrchestrationTaskMessage>(binaryData.ToArray());
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing && !disposed)
        {
            publisherOrchestrationTaskReply?.Dispose();
            publisherOrchestrationTaskEception?.Dispose();
            disposed = true;
        }
    }
}
