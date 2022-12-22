namespace PostBindOrchestrator.DomainLayer;

internal static class ValidatorApplicationInsightsSettingsConfig
{
    public static void Validate(ApplicationInsightsSettingsConfig applicationInsightsSettingsConfig)
    {
        var errorMessage = ValidateApplicationInsightsSettingsConfig(applicationInsightsSettingsConfig);

        if (errorMessage != null)
        {
            throw new ConfigurationSettingMissingException(errorMessage);
        }
    }

    private static string? ValidateApplicationInsightsSettingsConfig(ApplicationInsightsSettingsConfig applicationInsightsSettingsConfig)
    {
        return ValidatorString.Validate("AppInsightsConnectionString", applicationInsightsSettingsConfig.ConnectionString);
    }
}
