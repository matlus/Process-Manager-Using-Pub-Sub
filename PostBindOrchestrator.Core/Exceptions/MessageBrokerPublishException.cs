using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PostBindOrchestrator.Core;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class MessageBrokerPublishException : PostBindOrchestratorTechnicalBaseException
{
    public MessageBrokerPublishException()
    {
    }

    public MessageBrokerPublishException(string message)
        : base(message)
    {
    }

    public MessageBrokerPublishException(string message, Exception inner)
        : base(message, inner)
    {
    }

    private MessageBrokerPublishException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public override string Reason => "Failed to connect to message broker";
}
