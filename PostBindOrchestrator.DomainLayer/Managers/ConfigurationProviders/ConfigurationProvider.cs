using Microsoft.Extensions.Configuration;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;

public class ConfigurationProvider
{
    private readonly IConfiguration configuration;

    public ConfigurationProvider(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string GetRoleName()
    {
        return "PostBindOrchestrator";
    }

    public MessageBrokerSettings GetMessageBrokerSettings()
    {
        return MessageBrokerSettingsProvider.GetMessageBrokerSettings(configuration);
    }

    public ApplicationInsightsSettings GetApplicationInsightsSettings()
    {
        return ApplicationInsightsSettingsProvider.GetApplicationInsightsSettings(configuration);
    }

    public IConfiguration GetLoggingConfiguration()
    {
        return configuration.GetSection("Logging");
    }

    internal string? RetrieveConfigurationSettingValue(string key)
    {
        return configuration[key];
    }

    protected MessageBrokerSettingsConfig GetMessageBrokerSettingsPreValidated()
    {
        return MessageBrokerSettingsProvider.GetMessageBrokerSettingsPreValidated(configuration);
    }

    protected ApplicationInsightsSettingsConfig GetApplicationInsightsSettingsPreValidated()
    {
        return ApplicationInsightsSettingsProvider.GetApplicationInsightsSettingsPreValidated(configuration);
    }

    ////private string? RetrieveConfigurationSettingValueOrNull(string key)
    ////{
    ////    var value = RetrieveConfigurationSettingValue(key);
    ////    return ValidatorString.GetValueOrNull(value);
    ////}
}