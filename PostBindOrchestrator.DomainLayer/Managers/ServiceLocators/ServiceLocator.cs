
using PostBindOrchestrator.Core;
using Microsoft.Extensions.Logging;

namespace PostBindOrchestrator.DomainLayer;

public sealed class ServiceLocator : ServiceLocatorBase
{
    private ConfigurationProvider? configurationProvider;
    private LoggerProvider? loggerProvider;

    private ConfigurationProvider ConfigurationProvider
    {
        get
        {
            return configurationProvider ??= CreateConfigurationProvider();
        }
    }

    private LoggerProvider LoggerProvider
    {
        get
        {
            return loggerProvider ??= new LoggerProvider("PostBindOrchestrator.DomainLayer", ConfigurationProvider.GetLoggingConfiguration, ConfigurationProvider.GetApplicationInsightsSettings().ConnectionString);
        }
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
        return LoggerProvider.CreateLogger();
    }
}
