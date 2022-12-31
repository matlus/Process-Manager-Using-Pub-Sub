using System.Text;
using Microsoft.Extensions.Configuration;

namespace PostBindOrchestrator.Core;

public static class MessageBrokerSettingsProvider
{
    private const string messageBrokerSettingsKey = "MessageBroker";
    private const string messageBrokerTypePropertyName = "MessageBrokerType";

    /// <summary> 
    ///  
    /// </summary>
    /// <example>
    /// In the appsettings.json file, the MessageBrokerSettings section looks like the example code below.
    /// Valid values for the "MessageBrokerType" property are those of enum <see cref="MessageBrokerType"/>
    /// <code>
    ///   "MessageBroker": {
    ///       "MessageBrokerConnectionString": "YourConnectionString",
    ///       "MessageBrokerType": "ServiceBus"
    ///   }
    /// </code>
    /// </example>
    /// <param name="configurationRoot">The <see cref="IConfigurationRoot"/></param>
    /// <param name="retrieveConfigurationSettingValueOrNull">A Func that returns the configuration setting value or null if the value is missing, empty or white spaces</param>
    /// <returns>A validated MessageBrokerSettings instance</returns>
    public static MessageBrokerSettings GetMessageBrokerSettings(IConfigurationRoot configurationRoot)
    {
        var messageBrokerSettingsConfig = GetMessageBrokerSettingsPreValidated(configurationRoot);
        Validate(messageBrokerSettingsConfig);
        return messageBrokerSettingsConfig;
    }

    public static MessageBrokerSettingsConfig GetMessageBrokerSettingsPreValidated(IConfigurationRoot configurationRoot)
    {
        var messageBrokerSettingsConfig = configurationRoot.GetSection(messageBrokerSettingsKey).Get<MessageBrokerSettingsConfig>();

        if (messageBrokerSettingsConfig is not null)
        {
            messageBrokerSettingsConfig.MessageBrokerType = GetMessageBrokerType(configurationRoot);
        }
        else
        {
            messageBrokerSettingsConfig = new MessageBrokerSettingsConfig();
        }

        return messageBrokerSettingsConfig;
    }

    private static MessageBrokerType GetMessageBrokerType(IConfigurationRoot configurationRoot)
    {
        var value = configurationRoot[$"{messageBrokerSettingsKey}:{messageBrokerTypePropertyName}"];
        var messageBrokerTypeString = ValidatorString.GetValueOrNull(value) ?? MessageBrokerType.ServiceBus.ToString();
        return (MessageBrokerType)Enum.Parse(typeof(MessageBrokerType), messageBrokerTypeString);
    }

    private static void Validate(MessageBrokerSettingsConfig messageBrokerSettingsConfig)
    {
        var errorMessages = new StringBuilder();

        ValidateMessageBrokerSettingsConfig(errorMessages, messageBrokerSettingsConfig);

        if (errorMessages.Length is not 0)
        {
            throw new ConfigurationSettingMissingException(errorMessages.ToString());
        }
    }

    private static void ValidateMessageBrokerSettingsConfig(StringBuilder errorMessages, MessageBrokerSettingsConfig messageBrokerSettingsConfig)
    {
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"MessageBrokerSettings.MessageBrokerConnectionString", messageBrokerSettingsConfig.MessageBrokerConnectionString));
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate("MessageBrokerSettings.MessageBrokerType", messageBrokerSettingsConfig.MessageBrokerType.ToString()));
    }
}