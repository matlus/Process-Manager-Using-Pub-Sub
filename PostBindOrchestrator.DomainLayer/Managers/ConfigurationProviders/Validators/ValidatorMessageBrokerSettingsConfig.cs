using System.Text;

namespace PostBindOrchestrator.DomainLayer;

internal static class ValidatorMessageBrokerSettingsConfig
{
    public static void Validate(MessageBrokerSettingsConfig messageBrokerSettingsConfig)
    {
        var errorMessages = new StringBuilder();

        ValidateMessageBrokerSettingsConfig(errorMessages, messageBrokerSettingsConfig);

        if (errorMessages.Length != 0)
        {
            throw new ConfigurationSettingMissingException(errorMessages.ToString());
        }
    }

    private static void ValidateMessageBrokerSettingsConfig(StringBuilder errorMessages, MessageBrokerSettingsConfig messageBrokerSettingsConfig)
    {
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate("MessageBrokerSettings.MessageBrokerConnectionString", messageBrokerSettingsConfig.MessageBrokerConnectionString));
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate("MessageBrokerSettings.MessageBrokerType", messageBrokerSettingsConfig.MessageBrokerType.ToString()));
    }
}
