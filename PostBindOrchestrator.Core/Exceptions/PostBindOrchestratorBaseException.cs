using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PostBindOrchestrator.Core;

[Serializable]
[ExcludeFromCodeCoverage]
public abstract class PostBindOrchestratorBaseException : Exception
{
    protected PostBindOrchestratorBaseException()
    {
    }

    protected PostBindOrchestratorBaseException(string message, RootCause? rootCause = default)
        : base(message) => RootCause = rootCause;

    protected PostBindOrchestratorBaseException(string message, Exception innerException, RootCause? rootCause = default)
        : base(message, innerException) => RootCause = rootCause;

    protected PostBindOrchestratorBaseException(SerializationInfo info, StreamingContext context)
    : base(info, context)
    {
    }

    public abstract string Reason { get; }
    public abstract string Category { get; }
    public RootCause? RootCause { get; }
}