using PostBindOrchestrator.DomainLayer;

namespace PostBindOrchestrator.Api;

public sealed class MessageBrokerWorker : BackgroundService
{
    private readonly DomainFacade domainFacade;
    private readonly ApplicationLogger applicationLogger;
    private bool disposed;

    public MessageBrokerWorker(DomainFacade domainFacade, ApplicationLogger applicationLogger)
    {
        this.domainFacade = domainFacade;
        this.applicationLogger = applicationLogger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await domainFacade.StartListening(stoppingToken);
        }
        catch (Exception e)
        {
            applicationLogger.LogError(
                LogEvent.OnStartListening,
                e,
                "{CorrelationId}, {MethodName}, {Step}, {ExceptionType}, {ExceptionMessage}",
                "Need CorrelationId Here",
                nameof(ExecuteAsync),
                nameof(MessageBrokerWorker),
                e.GetType().Name,
                e.Message);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        if (!disposed)
        {
            domainFacade.Dispose();
            disposed = true;
        }
    }
}
