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

    private static void LoadEnvironmentSpecificAppSettings(IConfigurationBuilder configurationBuilder)
    {
        var aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (aspNetCoreEnvironment is not null)
        {
            configurationBuilder.AddJsonFile($"appsettings.{aspNetCoreEnvironment}.json");
        }
    }

    public MessageBrokerSettings GetMessageBrokerSettings()
    {
        return MessageBrokerSettingsProvider.GetMessageBrokerSettings(configurationRoot);
    }

    public ApplicationInsightsSettings GetApplicationInsightsSettings()
    {
        return ApplicationInsightsSettingsProvider.GetApplicationInsightsSettings(configurationRoot);
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
        return MessageBrokerSettingsProvider.GetMessageBrokerSettingsPreValidated(configurationRoot);
    }

    protected ApplicationInsightsSettingsConfig GetApplicationInsightsSettingsPreValidated()
    {
        return ApplicationInsightsSettingsProvider.GetApplicationInsightsSettingsPreValidated(configurationRoot);
    }

    ////private string? RetrieveConfigurationSettingValueOrNull(string key)
    ////{
    ////    var value = RetrieveConfigurationSettingValue(key);
    ////    return ValidatorString.GetValueOrNull(value);
    ////}
}