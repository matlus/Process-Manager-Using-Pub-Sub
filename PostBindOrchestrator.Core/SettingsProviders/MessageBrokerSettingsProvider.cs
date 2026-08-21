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
    /// <param name="configuration">The <see cref="IConfiguration"/> to read the settings from</param>
    /// <returns>A validated MessageBrokerSettings instance</returns>
    public static MessageBrokerSettings GetMessageBrokerSettings(IConfiguration configuration)
    {
        var messageBrokerSettingsConfig = GetMessageBrokerSettingsUnValidated(configuration);
        Validate(messageBrokerSettingsConfig);
        return messageBrokerSettingsConfig;
    }

    public static MessageBrokerSettingsConfig GetMessageBrokerSettingsUnValidated(IConfiguration configuration)
    {
        var messageBrokerSettingsConfig = configuration.GetSection(messageBrokerSettingsKey).Get<MessageBrokerSettingsConfig>();

        if (messageBrokerSettingsConfig is not null)
        {
            messageBrokerSettingsConfig.MessageBrokerType = GetMessageBrokerType(configuration);
        }
        else
        {
            messageBrokerSettingsConfig = new MessageBrokerSettingsConfig();
        }

        return messageBrokerSettingsConfig;
    }

    private static MessageBrokerType GetMessageBrokerType(IConfiguration configuration)
    {
        var value = configuration[$"{messageBrokerSettingsKey}:{messageBrokerTypePropertyName}"];
        var messageBrokerTypeString = ValidatorString.GetValueOrNull(value);

        if (messageBrokerTypeString is null)
        {
            return MessageBrokerType.ServiceBus;
        }

        if (!Enum.TryParse<MessageBrokerType>(messageBrokerTypeString, ignoreCase: true, out var messageBrokerType))
        {
            throw new MessageBrokerTypeNotSupportedException(
                $"The configuration setting: \"{messageBrokerSettingsKey}:{messageBrokerTypePropertyName}\" has a value of: \"{messageBrokerTypeString}\" which is not a supported Message Broker type. Valid values are: {string.Join(", ", Enum.GetNames(typeof(MessageBrokerType)))}");
        }

        return messageBrokerType;
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
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{messageBrokerSettingsKey}.{nameof(MessageBrokerSettings.ConnectionString)}", messageBrokerSettingsConfig.ConnectionString));
    }
}
