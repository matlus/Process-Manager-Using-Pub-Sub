using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PostBindOrchestrator.Core;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class BrokerMessageReceivedException : PostBindOrchestratorTechnicalBaseException
{
    public BrokerMessageReceivedException()
    {
    }

    public BrokerMessageReceivedException(string message)
        : base(message)
    {
    }

    public BrokerMessageReceivedException(string message, Exception inner)
        : base(message, inner)
    {
    }

    private BrokerMessageReceivedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public override string Reason => "Message broker message received error";
}
