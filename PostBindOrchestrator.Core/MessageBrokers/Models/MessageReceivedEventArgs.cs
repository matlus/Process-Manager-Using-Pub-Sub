namespace PostBindOrchestrator.Core;

public sealed class MessageReceivedEventArgs : EventArgs
{
    public MessageReceivedEventArgs(BrokerMessage message, object acknowledgeToken, CancellationToken cancellationToken)
    {
        Message = message;
        AcknowledgeToken = acknowledgeToken;
        CancellationToken = cancellationToken;
    }

    public BrokerMessage Message { get; }

    public object AcknowledgeToken { get; }

    public CancellationToken CancellationToken { get; }
}