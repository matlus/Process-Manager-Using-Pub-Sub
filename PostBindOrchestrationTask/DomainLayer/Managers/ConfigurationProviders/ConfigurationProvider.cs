using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrationTask.DomainLayer;

public class ConfigurationProvider
{
    private readonly IConfiguration configuration;

    public ConfigurationProvider(IConfiguration configuration) => this.configuration = configuration;

    [ExcludeFromCodeCoverage]
    internal ConfigurationProvider(IConfigurationRoot configurationRoot) => configuration = configurationRoot;

    public MessageBrokerSettings GetMessageBrokerSettings()
    {
        return MessageBrokerSettingsProvider.GetMessageBrokerSettings(configuration);
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