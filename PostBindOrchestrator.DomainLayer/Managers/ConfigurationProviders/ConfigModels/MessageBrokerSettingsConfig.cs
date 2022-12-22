using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;

public sealed class MessageBrokerSettingsConfig
{
    public string? MessageBrokerConnectionString { get; set; }

    public MessageBrokerType MessageBrokerType { get; set; }
}
