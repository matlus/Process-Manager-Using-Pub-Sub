using Microsoft.Extensions.Configuration;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrationTask.DomainLayer;

public sealed class ConfigurationProvider
{
    private readonly IConfiguration configuration;

    private MessageBrokerSettings? messageBrokerSettings;

    public ConfigurationProvider(IConfiguration configuration) => this.configuration = configuration;

    public MessageBrokerSettings GetMessageBrokerSettings()
    {
        return messageBrokerSettings ??= MessageBrokerSettingsProvider.GetMessageBrokerSettings(configuration);
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