using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PostBindOrchestrator.Core;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class DateTimeOffsetParseException : PostBindOrchestratorBusinessBaseException
{
    public DateTimeOffsetParseException()
    {
    }

    public DateTimeOffsetParseException(string message)
        : base(message)
    {
    }

    public DateTimeOffsetParseException(string message, Exception inner)
        : base(message, inner)
    {
    }

    private DateTimeOffsetParseException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public override string Reason => "DateTime parse error";
}
