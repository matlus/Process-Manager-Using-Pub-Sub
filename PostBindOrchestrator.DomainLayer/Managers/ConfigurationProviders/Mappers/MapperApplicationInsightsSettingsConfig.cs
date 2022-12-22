using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;

internal static class MapperApplicationInsightsSettingsConfig
{
    public static ApplicationInsightsSettings MapToApplicationInsightsSettings(ApplicationInsightsSettingsConfig applicationInsightsSettingsConfig)
    {
        return new ApplicationInsightsSettings(applicationInsightsSettingsConfig.ConnectionString!);
    }
}
