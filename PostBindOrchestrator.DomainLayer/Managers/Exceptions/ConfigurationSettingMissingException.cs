using System.Runtime.Serialization;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;


[Serializable]
public class ConfigurationSettingMissingException : PostBindOrchestratorBusinessBaseException
{
	public ConfigurationSettingMissingException() { }
	public ConfigurationSettingMissingException(string message) : base(message) { }
	public ConfigurationSettingMissingException(string message, Exception inner) : base(message, inner) { }
	protected ConfigurationSettingMissingException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public override string Reason => "Configuration Setting Missing";
}