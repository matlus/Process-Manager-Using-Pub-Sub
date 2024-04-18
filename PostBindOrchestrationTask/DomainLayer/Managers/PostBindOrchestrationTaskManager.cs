using PostBindOrchestrator.Core;
using PostBindOrchestrator.Core.Models.OrchestrationMessages;

namespace PostBindOrchestrationTask.DomainLayer;

internal sealed class PostBindOrchestrationTaskManager : IDisposable
{
    private const string OrchestrationReplyTopicName = "pbo.orch.reply.topic";
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
            await publisherOrchestrationTaskReply.Initialize(messageBrokerSettings.ConnectionString, OrchestrationReplyTopicName, OrchestrationReplyQueueName);

            publisherOrchestrationTaskEception = serviceLocator.CreateMessageBrokerPublisher();
            await publisherOrchestrationTaskEception.Initialize(messageBrokerSettings.ConnectionString, OrchestrationExceptionTopicName, OrchestrationExceptionQueueName);

            publishersHaveBeenInitialized = true;
        }
    }

    public async Task StartTask(byte[] message, CancellationToken cancellationToken)
    {
        var logEvent = LogEvent.StartTask;
        var orchestrationTaskMessage = GetOrchestrationTaskMessage(message);
        logEvent = LogEvent.OrchestrationTaskMessageReceived;

        try
        {
            await InitializePublishers();
            logEvent = LogEvent.OrchestrationTaskStarted;

            logEvent = LogEvent.OrchestrationTaskStartRecord;
            // Create Orchestration Record in Db using orchestrationMessageTask
            // Do the Task
            logEvent = LogEvent.OrchestrationTaskFinishedRecord;
            // Record Task Finished in Db

            var messageId = Guid.NewGuid().ToString("N");
            logEvent = LogEvent.OrchestrationTaskReplyPublish;
            await publisherOrchestrationTaskReply!.Publish(new OrchestrationReplyMessage(
                messageId,
                orchestrationTaskMessage.CorrelationId,
                orchestrationTaskMessage.PolicyNumber,
                DateTimeOffset.UtcNow,
                PostBindOrchestrator.Core.OrchestrationTask.SendCoIDocument,
                "Completed"), messageId, orchestrationTaskMessage.CorrelationId);
        }
        catch (Exception e)
        {
            ApplicationLogger.LogError(logEvent, e, $"{nameof(StartTask)}:catch block", orchestrationTaskMessage);
            var exceptionData = (ExceptionData)e;

            var messageId = Guid.NewGuid().ToString("N");
            var orchestrationExceptionMessage = new OrchestrationExceptionMessage(
                messageId,
                orchestrationTaskMessage.CorrelationId,
                orchestrationTaskMessage.PolicyNumber,
                DateTimeOffset.UtcNow,
                OrchestrationTask.SendCoIDocument,
                logEvent,
                exceptionData);

            // TODO: Record Exception to Db
            //// await DataFacade.RecordTaskException(orchestrationExceptionMessage, exceptionData);
            await publisherOrchestrationTaskEception!.Publish(orchestrationExceptionMessage, messageId, orchestrationTaskMessage.CorrelationId);
        }
    }

    private static OrchestrationTaskMessage GetOrchestrationTaskMessage(byte[] message)
    {
        return ApplicationSerializer.Deserialize<OrchestrationTaskMessage>(message);
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
