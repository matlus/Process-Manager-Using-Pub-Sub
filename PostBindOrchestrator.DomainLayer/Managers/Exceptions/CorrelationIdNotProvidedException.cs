using System.Runtime.Serialization;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;

[Serializable]
public sealed class CorrelationIdNotProvidedException : PostBindOrchestratorBusinessBaseException
{
    public CorrelationIdNotProvidedException() { }
    public CorrelationIdNotProvidedException(string message) : base(message) { }
    public CorrelationIdNotProvidedException(string message, Exception inner) : base(message, inner) { }
    private CorrelationIdNotProvidedException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public override string Reason => "Correlation Id Not Provided";
}