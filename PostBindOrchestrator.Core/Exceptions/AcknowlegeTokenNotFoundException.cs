using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PostBindOrchestrator.Core;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class AcknowlegeTokenNotFoundException : PostBindOrchestratorTechnicalBaseException
{
    public AcknowlegeTokenNotFoundException()
    {
    }

    public AcknowlegeTokenNotFoundException(string message)
        : base(message)
    {
    }

    public AcknowlegeTokenNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }

    private AcknowlegeTokenNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public override string Reason => "Acknowlege Token Not Found.";
}
