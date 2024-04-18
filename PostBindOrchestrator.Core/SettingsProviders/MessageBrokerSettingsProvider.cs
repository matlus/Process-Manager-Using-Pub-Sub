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
    /// <param name="configurationRoot">The <see cref="IConfiguration"/></param>
    /// <param name="retrieveConfigurationSettingValueOrNull">A Func that returns the configuration setting value or null if the value is missing, empty or white spaces</param>
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
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{messageBrokerSettingsKey}.{nameof(MessageBrokerSettings.ConnectionString)}", messageBrokerSettingsConfig.ConnectionString));
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{messageBrokerSettingsKey}.{nameof(MessageBrokerSettings.MessageBrokerType)}", messageBrokerSettingsConfig.MessageBrokerType.ToString()));
    }
}