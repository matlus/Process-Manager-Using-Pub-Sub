namespace PostBindOrchestrator.Core;

public sealed record KeyVaultSettings(AzureAdApp AzureAdApp, ConfigProvider ConfigProvider, IEnumerable<Connection> Connections, Retry Retry);
public sealed record AzureAdApp(string TenantId, string AzureAdAppId, string ClientCertificateThumbprint);
public sealed record ConfigProvider(bool Enabled, TimeSpan ReloadInterval);
public sealed record Connection(string Name, string ServiceEndpoint, bool Enabled);
public sealed record Retry(TimeSpan MaxDelay, TimeSpan SeedDelay, double SeedMultiplier, TimeSpan MaxWait, int MaxRetries);