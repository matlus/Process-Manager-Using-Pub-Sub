using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PostBindOrchestrator.Core;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class MessageDeserializationFailedException : PostBindOrchestratorTechnicalBaseException
{
    public MessageDeserializationFailedException()
    {
    }

    public MessageDeserializationFailedException(string message)
        : base(message)
    {
    }

    public MessageDeserializationFailedException(string message, Exception inner)
        : base(message, inner)
    {
    }

    private MessageDeserializationFailedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public override string Reason => "Failed to deserialize the message.";
}
