using System.Diagnostics.CodeAnalysis;

namespace PostBindOrchestrator.DomainLayer;

public sealed class DomainFacade : IDisposable
{
    private bool disposed;
    private readonly ServiceLocatorBase serviceLocator;
    private PostBindOrchestrationManager? postBindOrchestrationManager;

    public DomainFacade()
          : this(new ServiceLocator())
    {
    }

    public DomainFacade(ServiceLocatorBase serviceLocator) => this.serviceLocator = serviceLocator;

    private PostBindOrchestrationManager PostBindOrchestrationManager
    {
        get
        {
            return postBindOrchestrationManager ??= new PostBindOrchestrationManager(serviceLocator);
        }
    }

    public Task StartListening(CancellationToken cancellationToken)
    {
        return PostBindOrchestrationManager.StartListening(cancellationToken);
    }

    public Task ProcessPostBind(string correlationId, string policyNumber, InterviewData interviewData)
    {
        return PostBindOrchestrationManager.ProcessPostBind(correlationId, policyNumber, interviewData);
    }

    public Task ProcessRevertToQuote(string correlationId, string policyNumber)
    {
        return PostBindOrchestrationManager.ProcessRevertToQuote(correlationId, policyNumber);
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
            postBindOrchestrationManager?.Dispose();
            disposed = true;
        }
    }
}
