using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;

internal sealed class PostBindOrchestrationManager : IDisposable
{
    private const string MessageBrokerTopicQueuePrefix = "pbo.orch";
    private const string MessageBrokerTopicSufix = "topic";
    private const string MessageBrokerQueueSufix = "queue";
    private const string OrchestrationReplyTopicName = $"{MessageBrokerTopicQueuePrefix}.reply.{MessageBrokerTopicSufix}";
    private const string OrchestrationReplyQueueName = $"{MessageBrokerTopicQueuePrefix}.reply.{MessageBrokerQueueSufix}";

    private readonly ServiceLocatorBase serviceLocator;
    private bool disposed;
    private ConfigurationProvider? configurationProvider;
    private ApplicationLogger? applicationLogger;
    private SubscriberBase? subscriberOrchestrationReply;
    private Dictionary<OrchestrationTask, PublisherBase>? taskPublishers;

    private ConfigurationProvider ConfigurationProvider => configurationProvider ??= serviceLocator.CreateConfigurationProvider();

    private ApplicationLogger ApplicationLogger => applicationLogger ??= new ApplicationLogger(serviceLocator.CreateLogger());

    public PostBindOrchestrationManager(ServiceLocatorBase serviceLocator) => this.serviceLocator = serviceLocator;

    public async Task ProcessPostBind(string correlationId, string policyNumber, InterviewData interviewData)
    {
        //// Validate formal arguments
        //// Should persist interview data in the database so Task Processors can access and simplifies "state" maintenace across threads (trigger and reply)

        await PublishTaskMessage(correlationId, policyNumber, OrchestrationTask.SendCoIDocument);
    }

    public Task ProcessRevertToQuote(string correlationId, string policyNumber)
    {
        //// Validate formal arguments
        return Task.CompletedTask;
    }

    public async Task StartListening(CancellationToken cancellationToken)
    {
        var messageBrokerSettings = ConfigurationProvider.GetMessageBrokerSettings();
        await InitializeTaskPublishers(messageBrokerSettings);

        subscriberOrchestrationReply = serviceLocator.CreateMessageBrokerSubscriber();
        await subscriberOrchestrationReply.Initialize(messageBrokerSettings.MessageBrokerConnectionString, OrchestrationReplyTopicName, OrchestrationReplyQueueName, cancellationToken);
        await subscriberOrchestrationReply.Subscribe(OnOrchestrationReplyMessageReceived, cancellationToken);        

        //// Start listening on the Exception Topic as well
    }

    private async Task InitializeTaskPublishers(MessageBrokerSettings messageBrokerSettings)
    {
        taskPublishers = new();

        var orchestrationTask = OrchestrationTask.SendCoIDocument;
        var publisherTask = serviceLocator.CreateMessageBrokerPublisher();
        await publisherTask.Initialize(messageBrokerSettings.MessageBrokerConnectionString, $"{MessageBrokerTopicQueuePrefix}.{orchestrationTask.ToString().ToLower()}.{MessageBrokerTopicSufix}" , OrchestrationReplyQueueName);
        taskPublishers.Add(orchestrationTask, publisherTask);

        orchestrationTask = OrchestrationTask.UpdateIds;
        publisherTask = serviceLocator.CreateMessageBrokerPublisher();
        await publisherTask.Initialize(messageBrokerSettings.MessageBrokerConnectionString, $"{MessageBrokerTopicQueuePrefix}.{orchestrationTask.ToString().ToLower()}.{MessageBrokerTopicSufix}", OrchestrationReplyQueueName);
        taskPublishers.Add(orchestrationTask, publisherTask);
    }

    private async Task OnOrchestrationReplyMessageReceived(SubscriberBase subscriber, MessageReceivedEventArgs messageReceivedEventArgs)
    {
        var brokerMessage = messageReceivedEventArgs.Message;
        OrchestrationReplyMessage? orchestrationReplyMessage = null;
        var logEvent = LogEvent.OrchestrationReplyMessageReceived;

        try
        {
            logEvent = LogEvent.DeSerializeMessage;
            orchestrationReplyMessage = ApplicationSerializer.Deserialize<OrchestrationReplyMessage>(brokerMessage.Body);

            var orchestrationTaskNext = GetNextOrchestrationTask(orchestrationReplyMessage.PolicyNumber, orchestrationReplyMessage.OrchestrationTask);
            logEvent = LogEvent.OrchestrationTaskStartPublish;
            await PublishTaskMessage(brokerMessage.CorrelationId, orchestrationReplyMessage.PolicyNumber, orchestrationTaskNext);
            //// Publish message to trigger next task.
            //// Orchestrate the next Task (Publish OrchestrationTaskMessage to appropriate topic)
            //// Use OrchestrationTaskTopicDictionary to map task to topic

            await subscriber.Acknowledge(messageReceivedEventArgs.AcknowledgeToken, messageReceivedEventArgs.CancellationToken);
        }
        catch (MessageDeserializationFailedException e)
        {
            ApplicationLogger.LogError(logEvent, e, nameof(OnOrchestrationReplyMessageReceived), brokerMessage);
        }
        catch (Exception e)
        {
            ApplicationLogger.LogError(logEvent, e, nameof(OnOrchestrationReplyMessageReceived), orchestrationReplyMessage!);
        }
    }

    private async Task PublishTaskMessage(string correlationId, string policyNumber, OrchestrationTask orchestrationTaskNext)
    {
        var publisherTask = taskPublishers![orchestrationTaskNext];
        var messageId = Guid.NewGuid().ToString("N");
        await publisherTask.Publish(new OrchestrationTaskMessage(messageId, correlationId, policyNumber, DateTimeOffset.UtcNow, orchestrationTaskNext), messageId, correlationId);
    }

    private static OrchestrationTask GetNextOrchestrationTask(string policyNumber, OrchestrationTask orchestrationTask)
    {
        // Of course some logic needs to be here
        return NextOrchestrationTaskEnumProvider.GetNext(orchestrationTask);
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
            if (taskPublishers is not null)
            {
                foreach (var publisher in taskPublishers.Values)
                {
                    publisher.Dispose();
                }
            }

            subscriberOrchestrationReply?.Dispose();
            disposed = true;
        }
    }
}
