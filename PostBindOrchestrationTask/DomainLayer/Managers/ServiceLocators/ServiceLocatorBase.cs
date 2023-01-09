using Microsoft.Extensions.Logging;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrationTask.DomainLayer;

public abstract class ServiceLocatorBase
{
    public ConfigurationProvider CreateConfigurationProvider()
    {
        return CreateConfigurationProviderCore();
    }

    public PublisherBase CreateMessageBrokerPublisher()
    {
        return CreateMessageBrokerPublisherCore();
    }

    public ILogger CreateLogger()
    {
        return CreateLoggerCore();
    }

    protected abstract ConfigurationProvider CreateConfigurationProviderCore();
    protected abstract PublisherBase CreateMessageBrokerPublisherCore();
    protected abstract ILogger CreateLoggerCore();
}
