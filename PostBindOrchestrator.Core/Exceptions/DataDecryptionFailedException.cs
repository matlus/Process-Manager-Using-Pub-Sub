using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PostBindOrchestrator.Core;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class DataDecryptionFailedException : PostBindOrchestratorTechnicalBaseException
{
    public DataDecryptionFailedException()
    {
    }

    public DataDecryptionFailedException(string message)
        : base(message)
    {
    }

    public DataDecryptionFailedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    private DataDecryptionFailedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public override string Reason => "Failed to decrypt data.";
}
