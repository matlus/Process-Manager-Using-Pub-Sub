using System.Runtime.Serialization;

namespace PostBindOrchestrator.Core;

public sealed class InvalidOrchestrationProgressException : PostBindOrchestratorBusinessBaseException
{
    public InvalidOrchestrationProgressException()
    {
    }

    public InvalidOrchestrationProgressException(string message)
        : base(message)
    {
    }

    public InvalidOrchestrationProgressException(string message, Exception inner)
        : base(message, inner)
    {
    }

    private InvalidOrchestrationProgressException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public override string Reason => "Invalid Orchestration Progress";
}
