using Microsoft.Extensions.Configuration;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;

public sealed class ConfigurationProvider
{
    private readonly IConfiguration configuration;

    private MessageBrokerSettings? messageBrokerSettings;
    private AzureAdSettings? azureAdSettings;
    private ApplicationInsightsSettings? applicationInsightsSettings;
    private KeyVaultSettings? keyVaultSettings;

    public ConfigurationProvider(IConfiguration configuration) => this.configuration = configuration;

    public static string GetRoleName() => "PostBindOrchestrator";

    public MessageBrokerSettings GetMessageBrokerSettings()
    {
        return messageBrokerSettings ??= MessageBrokerSettingsProvider.GetMessageBrokerSettings(configuration);
    }

    public AzureAdSettings GetAzureAdSettings()
    {
        return azureAdSettings ??= AzureAdSettingsProvider.GetAzureAdSettings(configuration);
    }

    public ApplicationInsightsSettings GetApplicationInsightsSettings()
    {
        return applicationInsightsSettings ??= ApplicationInsightsSettingsProvider.GetApplicationInsightsSettings(configuration);
    }

    public KeyVaultSettings GetKeyVaultSettings()
    {
        return keyVaultSettings ??= KeyVaultSettingsProvider.GetKeyVaultSettings(configuration);
    }

    public IConfiguration GetLoggingConfiguration()
    {
        return configuration.GetSection("Logging");
    }

    internal string? RetrieveConfigurationSettingValue(string key)
    {
        return configuration[key];
    }
}