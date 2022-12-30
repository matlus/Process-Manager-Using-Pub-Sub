using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;

public class ConfigurationProvider
{
    private readonly IConfigurationRoot configurationRoot;

    public ConfigurationProvider()
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile("appsettings.json");
        LoadEnvironmentSpecificAppSettings(configurationBuilder);
        configurationBuilder.AddEnvironmentVariables("PostBindOrc_");

        configurationRoot = configurationBuilder.Build();
    }

    [ExcludeFromCodeCoverage]
    internal ConfigurationProvider(IConfigurationRoot configurationRoot) => this.configurationRoot = configurationRoot;

    private static void LoadEnvironmentSpecificAppSettings(ConfigurationBuilder configurationBuilder)
    {
        var aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (aspNetCoreEnvironment != null)
        {
            configurationBuilder.AddJsonFile($"appsettings.{aspNetCoreEnvironment}.json");
        }
    }

    public MessageBrokerSettings GetMessageBrokerSettings()
    {
        var messageBrokerSettingsConfig = GetMessageBrokerSettingsPreValidated();
        ValidatorMessageBrokerSettingsConfig.Validate(messageBrokerSettingsConfig);
        return MapperMessageBrokerSettingsConfig.MapToMessageBrokerSettings(messageBrokerSettingsConfig);
    }

    public ApplicationInsightsSettings GetApplicationInsightsSettings()
    {
        var applicationInsightsSettingsConfig = GetApplicationInsightsSettingsPreValidated();
        ValidatorApplicationInsightsSettingsConfig.Validate(applicationInsightsSettingsConfig);
        return MapperApplicationInsightsSettingsConfig.MapToApplicationInsightsSettings(applicationInsightsSettingsConfig);
    }

    public IConfiguration GetLoggingConfiguration()
    {
        return configurationRoot.GetSection("Logging");
    }

    internal string? RetrieveConfigurationSettingValue(string key)
    {
        return configurationRoot[key];
    }

    protected MessageBrokerSettingsConfig GetMessageBrokerSettingsPreValidated()
    {
        const string messageBrokerSettingsKey = "MessageBroker";
        var messageBrokerSettingsConfig = configurationRoot.GetSection(messageBrokerSettingsKey).Get<MessageBrokerSettingsConfig>();

        if (messageBrokerSettingsConfig != null)
        {
            messageBrokerSettingsConfig.MessageBrokerType = GetMessageBrokerType();
        }
        else
        {
            messageBrokerSettingsConfig = new MessageBrokerSettingsConfig();
        }

        return messageBrokerSettingsConfig;
    }

    protected ApplicationInsightsSettingsConfig GetApplicationInsightsSettingsPreValidated()
    {
        const string applicationInsightsSettingsKey = "AppInsightsConnectionString";
        var applicationInsightsSettingsString = configurationRoot.GetSection(applicationInsightsSettingsKey).Get<string>();

        var applicationInsightsSettingsConfig = new ApplicationInsightsSettingsConfig
        {
            ConnectionString = applicationInsightsSettingsString
        };

        return applicationInsightsSettingsConfig;
    }

    private MessageBrokerType GetMessageBrokerType()
    {
        var messageBrokerTypeString = RetrieveConfigurationSettingValueOrNull($"MessageBroker:MessageBrokerType") ?? "ServiceBus";
        var messageBrokerType = (MessageBrokerType)Enum.Parse(typeof(MessageBrokerType), messageBrokerTypeString);
        return messageBrokerType;
    }

    private string? RetrieveConfigurationSettingValueOrNull(string key)
    {
        var value = RetrieveConfigurationSettingValue(key);
        return ValidatorString.DetermineNullEmptyOrWhiteSpaces(value) switch
        {
            StringState.Null or StringState.Empty or StringState.WhiteSpaces => null,
            _ => value,
        };
    }
}