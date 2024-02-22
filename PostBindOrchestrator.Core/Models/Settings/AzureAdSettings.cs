namespace PostBindOrchestrator.Core;

public sealed record AzureAdSettings(string Instance, string ClientId, string ClientSecret, string Domain, string TenantId, string Audience);