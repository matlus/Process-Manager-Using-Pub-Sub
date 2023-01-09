using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrationTask.DomainLayer;

public sealed class ServiceLocator : ServiceLocatorBase
{
    private readonly IConfiguration configuration;
    private readonly ILoggerFactory loggerFactory;
    private ConfigurationProvider? configurationProvider;

    private ConfigurationProvider ConfigurationProvider => configurationProvider ??= CreateConfigurationProvider();

    public ServiceLocator(IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        this.configuration = configuration;
        this.loggerFactory = loggerFactory;
    }

    protected override ConfigurationProvider CreateConfigurationProviderCore()
    {
        return new ConfigurationProvider(configuration);
    }

    protected override PublisherBase CreateMessageBrokerPublisherCore()
    {
        var messageBrokerSettings = ConfigurationProvider.GetMessageBrokerSettings();
        return MessageBrokerFactory.CreateMessageBrokerPublisher(messageBrokerSettings.MessageBrokerType);
    }

    protected override ILogger CreateLoggerCore()
    {
        return loggerFactory.CreateLogger("PostBindOrchestrator.DomainLayer");
    }
}