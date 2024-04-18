using Microsoft.Extensions.Configuration;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;

public class ConfigurationProvider
{
    private readonly IConfiguration configuration;

    public ConfigurationProvider(IConfiguration configuration) => this.configuration = configuration;

    public static string GetRoleName() => "PostBindOrchestrator";

    public MessageBrokerSettings GetMessageBrokerSettings()
    {
        return MessageBrokerSettingsProvider.GetMessageBrokerSettings(configuration);
    }

    public AzureAdSettings GetAzureAdSettings()
    {
        return AzureAdSettingsProvider.GetAzureAdSettings(configuration);
    }

    public ApplicationInsightsSettings GetApplicationInsightsSettings()
    {
        return ApplicationInsightsSettingsProvider.GetApplicationInsightsSettings(configuration);
    }

    public KeyVaultSettings GetKeyVaultSettings()
    {
        return KeyVaultSettingsProvider.GetKeyVaultSettings(configuration);
    }

    public IConfiguration GetLoggingConfiguration()
    {
        return configuration.GetSection("Logging");
    }

    internal string? RetrieveConfigurationSettingValue(string key)
    {
        return configuration[key];
    }

    protected MessageBrokerSettingsConfig GetMessageBrokerSettingsUnValidated()
    {
        return MessageBrokerSettingsProvider.GetMessageBrokerSettingsUnValidated(configuration);
    }

    protected ApplicationInsightsSettingsConfig GetApplicationInsightsSettingsUnValidated()
    {
        return ApplicationInsightsSettingsProvider.GetApplicationInsightsSettingsUnValidated(configuration);
    }

    protected AzureAdSettingsConfig GetAzureAdSettingsUnValidated()
    {
        return AzureAdSettingsProvider.GetAzureAdSettingsUnValidated(configuration);
    }
}