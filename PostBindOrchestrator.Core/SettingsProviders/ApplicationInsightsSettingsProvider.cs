using Microsoft.Extensions.Configuration;

namespace PostBindOrchestrator.Core;

public static class ApplicationInsightsSettingsProvider
{
    private const string applicationInsightsSettingsKey = "AppInsightsConnectionString";

    /// <summary> 
    ///  
    /// </summary>
    /// <example>
    /// In the appsettings.json file, the AppInsightsConnectionString section looks like the example code below.
    /// This setting and/or the value is optional.
    /// <code>
    ///   "AppInsightsConnectionString": "YourInstrumentationKey"
    /// </code>
    /// </example>
    /// <param name="configurationRoot">The <see cref="IConfigurationRoot"/></param>
    /// <param name="retrieveConfigurationSettingValueOrNull">A Func that returns the configuration setting value or null if the value is missing, empty or white spaces</param>
    /// <returns>A validated MessageBrokerSettings instance</returns>
    public static ApplicationInsightsSettings GetApplicationInsightsSettings(IConfigurationRoot configurationRoot)
    {
        var applicationInsightsSettingsConfig = GetApplicationInsightsSettingsPreValidated(configurationRoot);
        Validate(applicationInsightsSettingsConfig);
        return applicationInsightsSettingsConfig;
    }

    public static ApplicationInsightsSettingsConfig GetApplicationInsightsSettingsPreValidated(IConfigurationRoot configurationRoot)
    {
        var applicationInsightsSettingsString = configurationRoot.GetSection(applicationInsightsSettingsKey).Get<string>();

        var applicationInsightsSettingsConfig = new ApplicationInsightsSettingsConfig
        {
            ConnectionString = applicationInsightsSettingsString
        };

        return applicationInsightsSettingsConfig;
    }

    private static void Validate(ApplicationInsightsSettingsConfig applicationInsightsSettingsConfig)
    {
        var errorMessage = ValidateApplicationInsightsSettingsConfig(applicationInsightsSettingsConfig);

        if (errorMessage is not null)
        {
            throw new ConfigurationSettingMissingException(errorMessage);
        }
    }

    private static string? ValidateApplicationInsightsSettingsConfig(ApplicationInsightsSettingsConfig applicationInsightsSettingsConfig)
    {
        return ValidatorString.Validate("AppInsightsConnectionString", applicationInsightsSettingsConfig.ConnectionString);
    }
}