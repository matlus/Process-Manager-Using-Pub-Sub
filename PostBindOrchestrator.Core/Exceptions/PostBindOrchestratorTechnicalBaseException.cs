﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PostBindOrchestrator.Core;

[Serializable]
[ExcludeFromCodeCoverage]
public abstract class PostBindOrchestratorTechnicalBaseException : PostBindOrchestratorBaseException
{
    protected PostBindOrchestratorTechnicalBaseException()
    {
    }

    protected PostBindOrchestratorTechnicalBaseException(string message)
        : base(message)
    {
    }

    protected PostBindOrchestratorTechnicalBaseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected PostBindOrchestratorTechnicalBaseException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public sealed override string Category => "Technical";
}
