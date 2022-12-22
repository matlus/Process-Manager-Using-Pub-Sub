using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PostBindOrchestrator.Core;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class DataEncryptionFailedException : PostBindOrchestratorTechnicalBaseException
{
    public DataEncryptionFailedException()
    {
    }

    public DataEncryptionFailedException(string message) 
        : base(message)
    {
    }

    public DataEncryptionFailedException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    private DataEncryptionFailedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public override string Reason => "Failed to encrypt data.";
}
