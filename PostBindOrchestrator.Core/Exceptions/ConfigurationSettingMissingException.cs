using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PostBindOrchestrator.Core;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class ConfigurationSettingMissingException : PostBindOrchestratorTechnicalBaseException
{
    public ConfigurationSettingMissingException()
    {
    }

    public ConfigurationSettingMissingException(string message)
        : base(message)
    {
    }

    public ConfigurationSettingMissingException(string message, Exception inner)
        : base(message, inner)
    {
    }

    private ConfigurationSettingMissingException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public override string Reason => "Configuration setting missing";
}
