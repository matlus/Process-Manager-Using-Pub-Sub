using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PostBindOrchestrator.Core;

[Serializable]
[ExcludeFromCodeCoverage]
public abstract class PostBindOrchestratorBusinessBaseException : PostBindOrchestratorBaseException
{
    protected PostBindOrchestratorBusinessBaseException()
    {
    }

    protected PostBindOrchestratorBusinessBaseException(string message)
        : base(message)
    {
    }

    protected PostBindOrchestratorBusinessBaseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected PostBindOrchestratorBusinessBaseException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
