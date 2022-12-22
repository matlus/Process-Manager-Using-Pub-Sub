using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PostBindOrchestrator.Core;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class MessageBrokerTypeNotSupportedException : PostBindOrchestratorTechnicalBaseException
{
    public MessageBrokerTypeNotSupportedException()
    {
    }

    public MessageBrokerTypeNotSupportedException(string message)
        : base(message)
    {
    }

    public MessageBrokerTypeNotSupportedException(string message, Exception inner)
        : base(message, inner)
    {
    }

    private MessageBrokerTypeNotSupportedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public override string Reason => "Message Broker type is not supported.";
}
