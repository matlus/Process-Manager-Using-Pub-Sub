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

    protected PostBindOrchestratorBaseException(string message)
        : base(message)
    {
    }

    protected PostBindOrchestratorBaseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected PostBindOrchestratorBaseException(SerializationInfo info, StreamingContext context)
    : base(info, context)
    {
    }

    public abstract string Reason { get; }
}