namespace PostBindOrchestrator.Core;

public sealed class MessageReceivedEventArgs : EventArgs
{
    public MessageReceivedEventArgs(BrokerMessage message, string acknowledgeToken, CancellationToken cancellationToken)
    {
        Message = message;
        AcknowledgeToken = acknowledgeToken;
        CancellationToken = cancellationToken;
    }

    public BrokerMessage Message { get; }

    public string AcknowledgeToken { get; }

    public CancellationToken CancellationToken { get; }
}