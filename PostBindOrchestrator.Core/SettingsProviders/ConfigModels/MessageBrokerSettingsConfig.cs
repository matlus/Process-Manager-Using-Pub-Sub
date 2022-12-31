namespace PostBindOrchestrator.Core;

public sealed class MessageBrokerSettingsConfig
{
    public string? MessageBrokerConnectionString { get; set; }

    public MessageBrokerType MessageBrokerType { get; set; }

    public static implicit operator MessageBrokerSettings(MessageBrokerSettingsConfig messageBrokerSettingsConfig)
    {
        return new MessageBrokerSettings(messageBrokerSettingsConfig.MessageBrokerConnectionString!, messageBrokerSettingsConfig.MessageBrokerType);
    }
}
