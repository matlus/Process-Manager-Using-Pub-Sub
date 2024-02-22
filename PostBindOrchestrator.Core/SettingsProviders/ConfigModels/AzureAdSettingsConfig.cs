namespace PostBindOrchestrator.Core;

public sealed class AzureAdSettingsConfig
{
    public string? Instance { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Domain { get; set; }
    public string? TenantId { get; set; }
    public string? Audience { get; set; }

    public static implicit operator AzureAdSettings(AzureAdSettingsConfig azureAdSettingsConfig)
    {
        return new AzureAdSettings(
            azureAdSettingsConfig.Instance!,
            azureAdSettingsConfig.ClientId!,
            azureAdSettingsConfig.ClientSecret!,
            azureAdSettingsConfig.Domain!,
            azureAdSettingsConfig.TenantId!,
            azureAdSettingsConfig.Audience!);
    }
}

