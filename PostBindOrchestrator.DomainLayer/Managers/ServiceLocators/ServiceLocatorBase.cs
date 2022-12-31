using Microsoft.Extensions.Logging;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;

public abstract class ServiceLocatorBase : IDisposable
{
    public ConfigurationProvider CreateConfigurationProvider()
    {
        return CreateConfigurationProviderCore();
    }

    public SubscriberBase CreateMessageBrokerSubscriber()
    {
        return CreateMessageBrokerSubscriberCore();
    }

    public PublisherBase CreateMessageBrokerPublisher()
    {
        return CreateMessageBrokerPublisherCore();
    }

    public ILogger CreateLogger()
    {
        return CreateLoggerCore();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected abstract void Dispose(bool disposing);
    protected abstract ConfigurationProvider CreateConfigurationProviderCore();
    protected abstract SubscriberBase CreateMessageBrokerSubscriberCore();
    protected abstract PublisherBase CreateMessageBrokerPublisherCore();
    protected abstract ILogger CreateLoggerCore();
}
