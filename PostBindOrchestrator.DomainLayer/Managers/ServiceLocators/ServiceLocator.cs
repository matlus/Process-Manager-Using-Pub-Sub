using Microsoft.Extensions.Logging;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;

public sealed class ServiceLocator : ServiceLocatorBase
{
    private readonly ILoggerFactory loggerFactory;
    private ConfigurationProvider? configurationProvider;

    private ConfigurationProvider ConfigurationProvider => configurationProvider ??= CreateConfigurationProvider();

    public ServiceLocator(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
    }

    protected override ConfigurationProvider CreateConfigurationProviderCore()
    {
        return new ConfigurationProvider();
    }

    protected override PublisherBase CreateMessageBrokerPublisherCore()
    {
        var messageBrokerSettings = ConfigurationProvider.GetMessageBrokerSettings();
        return MessageBrokerFactory.CreateMessageBrokerPublisher(messageBrokerSettings.MessageBrokerType);
    }

    protected override SubscriberBase CreateMessageBrokerSubscriberCore()
    {
        var messageBrokerSettings = ConfigurationProvider.GetMessageBrokerSettings();
        return MessageBrokerFactory.CreateMessageBrokerSubscriber(messageBrokerSettings.MessageBrokerType);
    }

    protected override ILogger CreateLoggerCore()
    {
        return loggerFactory.CreateLogger("PostBindOrchestrator.DomainLayer");
    }
}