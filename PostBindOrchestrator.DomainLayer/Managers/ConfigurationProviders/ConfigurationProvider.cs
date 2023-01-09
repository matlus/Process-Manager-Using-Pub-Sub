using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;

public class ConfigurationProvider
{
    private readonly IConfiguration configuration;

    public ConfigurationProvider()
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile("appsettings.json");
        LoadEnvironmentSpecificAppSettings(configurationBuilder);
        configurationBuilder.AddEnvironmentVariables("PostBindOrc_");
        configuration = configurationBuilder.Build();
    }

    [ExcludeFromCodeCoverage]
    internal ConfigurationProvider(IConfigurationRoot configurationRoot) => configuration = configurationRoot;

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