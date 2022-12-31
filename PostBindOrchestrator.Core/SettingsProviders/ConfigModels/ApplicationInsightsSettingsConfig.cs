namespace PostBindOrchestrator.Core;

public sealed class ApplicationInsightsSettingsConfig
{
    public string? ConnectionString { get; set; }

    public static implicit operator ApplicationInsightsSettings(ApplicationInsightsSettingsConfig applicationInsightsSettingsConfig)
    {
        return new ApplicationInsightsSettings(applicationInsightsSettingsConfig.ConnectionString!);
    }
}
