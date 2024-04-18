using System.Text;
using Microsoft.Extensions.Configuration;

namespace PostBindOrchestrator.Core;

public static class KeyVaultSettingsProvider
{
    public const string KeyVaultSettingsKey = "KeyVault";    
    private const string ConnectionsKey = "Connections";

    /// <summary> 
    ///  
    /// </summary>
    /// <example>
    /// In the appsettings.json file, the KeyVaultSettings section looks like the example code below.
    /// <code>
    ///"KeyVault": {
    ///        "ConfigProvider": {
    ///            "Enabled": true,
    ///            "ReloadInterval": "01:00:00"
    ///        },
    ///        "AzureAdApp": {
    ///            "TenantId": "f728217d-0b71-43fd-b1a3-24cbc2bb2bfb",
    ///            "AzureAdAppId": "7f754768-81a4-4c0b-98b6-c0a559dc9c67",
    ///            "ClientCertificateThumbprint": "501caef4c6b84567af588a85b880bff5"
    ///        },
    ///        "Connections": [
    ///            {
    ///                "Name": "Connection1",
    ///                "ServiceEndpoint": "https://yourkeyvalt.vault.azure.net/",
    ///                "Enabled": true
    ///            }
    ///        ],
    ///        "Retry": {
    ///            "MaxDelay": "00:00:10",
    ///            "SeedDelay": "00:00:00.1",
    ///            "SeedMultiplier": 1.2,
    ///            "MaxWait": "00:02:00",
    ///            "MaxRetries": 6
    ///        }
    ///    }
    /// </code>
    /// </example>
    /// <param name="configurationRoot">The <see cref="IConfiguration"/></param>
    /// <returns>A validated KeyVaultSettings instance</returns>
    public static KeyVaultSettings GetKeyVaultSettings(IConfiguration configuration)
    {
        var keyVaultSettingsConfig = GetKeyValueSettingsUnValidated(configuration);
        Validate(keyVaultSettingsConfig);
        return keyVaultSettingsConfig;
    }

    public static KeyVaultSettingsConfig GetKeyValueSettingsUnValidated(IConfiguration configuration)
    {
        var keyVaultSettingsConfig = configuration.GetSection(KeyVaultSettingsKey).Get<KeyVaultSettingsConfig>();
        return keyVaultSettingsConfig ?? new KeyVaultSettingsConfig(default!, default!, Enumerable.Empty<ConnectionConfig>(), default!);
    }

    private static void Validate(KeyVaultSettingsConfig keyVaultSettingsConfig)
    {
        var errorMessages = new StringBuilder();
        ValidateAzureAdAppConfig(errorMessages, keyVaultSettingsConfig.AzureAdApp);
        ValidateConnectionsConfig(errorMessages, keyVaultSettingsConfig.Connections);
        ValidateConfigProviderConfig(errorMessages, keyVaultSettingsConfig.ConfigProvider);
        ValidateRetryConfig(errorMessages, keyVaultSettingsConfig.Retry);

        if (errorMessages.Length is not 0)
        {
            throw new ConfigurationSettingMissingException(errorMessages.ToString());
        }
    }

    private static void ValidateAzureAdAppConfig(StringBuilder errorMessages, AzureAdAppConfig? azureAdAppConfig)
    {
        if (azureAdAppConfig is null)
        {
            errorMessages.AppendLine(
                $"The Configuration Setting/Key: {KeyVaultSettingsKey}.{nameof(AzureAdApp)}, is missing and must be present with " +
                $"valid properties such as {nameof(AzureAdApp)}.{nameof(AzureAdApp.TenantId)}, {nameof(AzureAdApp)}.{nameof(AzureAdApp.AzureAdAppId)}, {nameof(AzureAdApp)}.{nameof(AzureAdApp.ClientCertificateThumbprint)} " +
                $"and values for each.");
            return;
        }

        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{KeyVaultSettingsKey}.{nameof(AzureAdApp)}.{nameof(AzureAdApp.TenantId)}", azureAdAppConfig.TenantId));
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{KeyVaultSettingsKey}.{nameof(AzureAdApp)}.{nameof(AzureAdApp.AzureAdAppId)}", azureAdAppConfig.AzureAdAppId));
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{KeyVaultSettingsKey}.{nameof(AzureAdApp)}.{nameof(AzureAdApp.ClientCertificateThumbprint)}", azureAdAppConfig.ClientCertificateThumbprint));        
    }

    private static void ValidateConnectionsConfig(StringBuilder errorMessages, IEnumerable<ConnectionConfig>? connections)
    {
        if (connections is null || connections.Count() == 0)
        {
            errorMessages.AppendLine(
                $"The Configuration Setting/Key: {KeyVaultSettingsKey}.{ConnectionsKey}, is missing and must be present with " +
                $"valid properties such as {nameof(Connection)}s.{nameof(Connection.Name)}, {nameof(Connection)}s.{nameof(Connection.ServiceEndpoint)}, {nameof(Connection)}s.{nameof(Connection.Enabled)} " +
                $"and values for each.");
            return;
        }

        foreach (var connection in connections)
        {
            errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{KeyVaultSettingsKey}.{ConnectionsKey}.{nameof(Connection.Name)}", connection.Name));
            errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{KeyVaultSettingsKey}.{ConnectionsKey}.{nameof(Connection.ServiceEndpoint)}", connection.ServiceEndpoint));
            errorMessages.AppendLineIfNotNull(ValidatorBoolean.Validate($"{KeyVaultSettingsKey}.{ConnectionsKey}.{nameof(Connection.Enabled)}", connection.Enabled));
        }
    }

    private static void ValidateConfigProviderConfig(StringBuilder errorMessages, ConfigProviderConfig? configProvider)
    {
        if (configProvider is null)
        {
            errorMessages.AppendLine(
                $"The Configuration Setting/Key: {KeyVaultSettingsKey}.{nameof(ConfigProvider)}, is missing and must be present with " +
                $"valid properties such as {nameof(ConfigProvider)}.{nameof(ConfigProvider.Enabled)} and {nameof(ConfigProvider)}.{nameof(ConfigProvider.ReloadInterval)} " +
                $"and values for each.");
            return;
        }

        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{KeyVaultSettingsKey}.{nameof(ConfigProvider)}.{nameof(ConfigProvider.Enabled)}", configProvider.Enabled));
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{KeyVaultSettingsKey}.{nameof(ConfigProvider)}.{nameof(ConfigProvider.ReloadInterval)}", configProvider.ReloadInterval));
    }

    private static void ValidateRetryConfig(StringBuilder errorMessages, RetryConfig? retry)
    {
        if (retry is null)
        {
            errorMessages.AppendLine(
                $"The Configuration Setting/Key: {KeyVaultSettingsKey}.{nameof(Retry)}, is missing and must be present with " +
                $"valid properties such as {nameof(Retry)}.{nameof(Retry.MaxDelay)}, {nameof(Retry)}.{nameof(Retry.SeedDelay)}, {nameof(Retry)}.{nameof(Retry.SeedMultiplier)}, {nameof(Retry)}.{nameof(Retry.MaxWait)} and {nameof(Retry)}.{nameof(Retry.MaxRetries)} " +
                $"and values for each.");
            return;
        }

        errorMessages.AppendLineIfNotNull(ValidatorTimeSpan.Validate($"{KeyVaultSettingsKey}.{nameof(Retry)}.{nameof(Retry.MaxDelay)}", retry.MaxDelay));
        errorMessages.AppendLineIfNotNull(ValidatorTimeSpan.Validate($"{KeyVaultSettingsKey}.{nameof(Retry)}.{nameof(Retry.SeedDelay)}", retry.SeedDelay));
        errorMessages.AppendLineIfNotNull(ValidatorDouble.Validate($"{KeyVaultSettingsKey}.{nameof(Retry)}.{nameof(Retry.SeedMultiplier)}", retry.SeedMultiplier));
        errorMessages.AppendLineIfNotNull(ValidatorTimeSpan.Validate($"{KeyVaultSettingsKey}.{nameof(Retry)}.{nameof(Retry.MaxWait)}", retry.MaxWait));
        errorMessages.AppendLineIfNotNull(ValidatorInt.Validate($"{KeyVaultSettingsKey}.{nameof(Retry)}.{nameof(Retry.MaxRetries)}", retry.MaxRetries));
    }
}