using Microsoft.Extensions.Configuration;

namespace PostBindOrchestrator.Core;

public static class ApplicationInsightsSettingsProvider
{
    private const string applicationInsightsSettingsKey = "ApplicationInsights";

    /// <summary> 
    ///  
    /// </summary>
    /// <example>
    /// In the appsettings.json file, the ApplicationInsights section looks like the example code below.
    /// This setting and/or the value is optional.
    /// <code>
    ///   "ApplicationInsights": {
    ///       "ConnectionString": "YourConnectionString"
    ///   }
    /// </code>
    /// </example>
    /// <param name="configuration">The <see cref="IConfiguration"/> to read the settings from</param>
    /// <returns>A validated ApplicationInsightsSettings instance</returns>
    public static ApplicationInsightsSettings GetApplicationInsightsSettings(IConfiguration configuration)
    {
        var applicationInsightsSettingsConfig = GetApplicationInsightsSettingsUnValidated(configuration);
        Validate(applicationInsightsSettingsConfig);
        return applicationInsightsSettingsConfig;
    }

    public static ApplicationInsightsSettingsConfig GetApplicationInsightsSettingsUnValidated(IConfiguration configuration)
    {
        var applicationInsightsSettingsConfig = configuration.GetSection(applicationInsightsSettingsKey).Get<ApplicationInsightsSettingsConfig>();
        return applicationInsightsSettingsConfig ?? new ApplicationInsightsSettingsConfig();
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
        return ValidatorString.Validate($"{applicationInsightsSettingsKey}.{nameof(ApplicationInsightsSettings.ConnectionString)}", applicationInsightsSettingsConfig.ConnectionString);
    }
}
