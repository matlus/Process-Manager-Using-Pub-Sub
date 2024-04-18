namespace PostBindOrchestrator.Core;

public sealed class KeyVaultSettingsConfig
{
    public AzureAdAppConfig? AzureAdApp { get; set; }
    public ConfigProviderConfig? ConfigProvider { get; set; }
    public IEnumerable<ConnectionConfig>? Connections { get; set; }
    public RetryConfig? Retry { get; set; }

    public KeyVaultSettingsConfig()
    {        
    }

    public KeyVaultSettingsConfig(AzureAdAppConfig azureAdApp, ConfigProviderConfig configProvider, IEnumerable<ConnectionConfig> connections, RetryConfig retry)
    {
        AzureAdApp = azureAdApp;
        ConfigProvider = configProvider;
        Connections = connections;
        Retry = retry;
    }

    public static implicit operator KeyVaultSettings(KeyVaultSettingsConfig keyVaultSettingsConfig)
    {
        return new KeyVaultSettings(
            keyVaultSettingsConfig.AzureAdApp!,
            keyVaultSettingsConfig.ConfigProvider!,
            keyVaultSettingsConfig.Connections!.Select(c => (Connection)c),
            keyVaultSettingsConfig.Retry!);
    }
}

public sealed class AzureAdAppConfig
{
    public string? TenantId { get; set; }
    public string? AzureAdAppId { get; set; }
    public string? ClientCertificateThumbprint { get; set; }

    public AzureAdAppConfig(string? tenantId = null, string? azureAdAppId = null, string? clientCertificateThumbprint = null)
    {
        TenantId = tenantId;
        AzureAdAppId = azureAdAppId;
        ClientCertificateThumbprint = clientCertificateThumbprint;
    }

    public static implicit operator AzureAdApp(AzureAdAppConfig azureAdAppConfig)
    {
        return new AzureAdApp(
            azureAdAppConfig.TenantId!,
            azureAdAppConfig.AzureAdAppId!,
            azureAdAppConfig.ClientCertificateThumbprint!);
    }
}

public sealed class ConfigProviderConfig
{
    public string? Enabled { get; set; }
    public string? ReloadInterval { get; set; }

    public ConfigProviderConfig(string? enabled = null, string? reloadInterval = null)
    {
        Enabled = enabled;
        ReloadInterval = reloadInterval;
    }

    public static implicit operator ConfigProvider(ConfigProviderConfig configProviderConfig)
    {
        return new ConfigProvider(
            bool.Parse(configProviderConfig.Enabled!),
            TimeSpan.Parse(configProviderConfig.ReloadInterval!, Thread.CurrentThread.CurrentCulture));
    }
}

public sealed class ConnectionConfig
{
    public string? Name { get; set; }
    public string? ServiceEndpoint { get; set; }
    public string? Enabled { get; set; }

    public ConnectionConfig(string? name = null, string? serviceEndpoint = null, string? enabled = null)
    {
        Name = name;
        ServiceEndpoint = serviceEndpoint;
        Enabled = enabled;
    }

    public static implicit operator Connection(ConnectionConfig connectionConfig)
    {
        return new Connection(
            connectionConfig.Name!,
            connectionConfig.ServiceEndpoint!,
            bool.Parse(connectionConfig.Enabled!));
    }
}

public sealed class RetryConfig
{
    public string? MaxDelay { get; set; }
    public string? SeedDelay { get; set; }
    public string? SeedMultiplier { get; set; }
    public string? MaxWait { get; set; }
    public string? MaxRetries { get; set; }

    public RetryConfig(string? maxDelay = null, string? seedDelay = null, string? seedMultiplier = null, string? maxWait = null, string? maxRetries = null)
    {
        MaxDelay = maxDelay;
        SeedDelay = seedDelay;
        SeedMultiplier = seedMultiplier;
        MaxWait = maxWait;
        MaxRetries = maxRetries;
    }

    public static implicit operator Retry(RetryConfig retryConfig)
    {
        return new Retry(
            TimeSpan.Parse(retryConfig.MaxDelay!, Thread.CurrentThread.CurrentCulture),
            TimeSpan.Parse(retryConfig.SeedDelay!, Thread.CurrentThread.CurrentCulture),
            double.Parse(retryConfig.SeedMultiplier!),
            TimeSpan.Parse(retryConfig.MaxWait!, Thread.CurrentThread.CurrentCulture),
            int.Parse(retryConfig.MaxRetries!));
    }
}