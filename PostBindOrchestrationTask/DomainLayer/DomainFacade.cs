using System.Diagnostics.CodeAnalysis;
using Azure.Messaging.ServiceBus;

namespace PostBindOrchestrationTask.DomainLayer;

public sealed class DomainFacade : IDisposable
{
    private bool disposed;
    private readonly ServiceLocatorBase serviceLocator;
    private PostBindOrchestrationTaskManager? postBindOrchestrationTaskManager;

    public DomainFacade(ServiceLocatorBase serviceLocator) => this.serviceLocator = serviceLocator;

    private PostBindOrchestrationTaskManager PostBindOrchestrationTaskManager
    {
        get
        {
            return postBindOrchestrationTaskManager ??= new PostBindOrchestrationTaskManager(serviceLocator);
        }
    }

    public Task StartTask(ServiceBusReceivedMessage serviceBusReceivedMessage, CancellationToken cancellationToken)
    {
        return PostBindOrchestrationTaskManager.StartTask(serviceBusReceivedMessage, cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    [ExcludeFromCodeCoverage]
    private void Dispose(bool disposing)
    {
        if (disposing && !disposed)
        {
            postBindOrchestrationTaskManager?.Dispose();
            disposed = true;
        }
    }
}
