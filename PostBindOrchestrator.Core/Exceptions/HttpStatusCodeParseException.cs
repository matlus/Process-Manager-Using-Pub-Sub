using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PostBindOrchestrator.Core;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class HttpStatusCodeParseException : PostBindOrchestratorBusinessBaseException
{
    public HttpStatusCodeParseException()
    {
    }

    public HttpStatusCodeParseException(string message)
        : base(message)
    {
    }

    public HttpStatusCodeParseException(string message, Exception inner)
        : base(message, inner)
    {
    }

    private HttpStatusCodeParseException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public override string Reason => "HTTP status code parse error";
}