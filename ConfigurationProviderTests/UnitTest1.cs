using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Extensions.Configuration;
using PostBindOrchestrator.Core;
using Testing.Core;

namespace ConfigurationProviderTests;

[TestClass]
public class UnitTest1
{
    private static readonly string azureAdAppId = Guid.NewGuid().ToString().ToLower();
    private static readonly string tenantId = Guid.NewGuid().ToString().ToLower();
    private static readonly string clientCertificateThumbprint = Guid.NewGuid().ToString("N").ToLower();

    private readonly ModelBuilder<KeyVaultSettingsConfig> keyVaultSettingsConfigBuilder;
    private readonly ModelBuilder<AzureAdAppConfig> azureAdAppConfigBuilder;
    private readonly ModelBuilder<ConnectionConfig> connectionsConfigBuilder;
    private readonly ModelBuilder<ConfigProviderConfig> configProviderConfigBuilder;
    private readonly ModelBuilder<RetryConfig> retryConfigBuilder;

    public UnitTest1()
    {
        azureAdAppConfigBuilder = InitializeAzureAdAppConfigBuilder();
        connectionsConfigBuilder = InitializeConnectionsConfigBuilder();
        configProviderConfigBuilder = InitializeConfigProviderConfigBuilder();
        retryConfigBuilder = InitializeRetryConfigBuilder();
        keyVaultSettingsConfigBuilder = InitializeKeyVaultSettingsConfigBuilder();
    }

    private static ModelBuilder<AzureAdAppConfig> InitializeAzureAdAppConfigBuilder()
    {
        return new ModelBuilder<AzureAdAppConfig>()
            .For(m => m.AzureAdAppId, () => azureAdAppId)
            .For(m => m.TenantId, () => tenantId)
            .For(m => m.ClientCertificateThumbprint, () => clientCertificateThumbprint);
    }

    private static ModelBuilder<ConnectionConfig> InitializeConnectionsConfigBuilder()
    {
        return new ModelBuilder<ConnectionConfig>()
            .For(m => m.ServiceEndpoint, () => "https://yourkeyvalt.vault.azure.net/")
            .For(m => m.Name, () => "Connection1")
            .For(m => m.Enabled, () => "true");
    }

    private static ModelBuilder<ConfigProviderConfig> InitializeConfigProviderConfigBuilder()
    {
        return new ModelBuilder<ConfigProviderConfig>()
            .For(m => m.Enabled, () => "true")
            .For(m => m.ReloadInterval, () => "00:30:00");
    }

    private static ModelBuilder<RetryConfig> InitializeRetryConfigBuilder()
    {
        return new ModelBuilder<RetryConfig>()
            .For(m => m.SeedMultiplier, () => "1.2")
            .For(m => m.SeedDelay, () => "00:00:00.1")
            .For(m => m.MaxRetries, () => "6")
            .For(m => m.MaxDelay, () => "00:30:00")
            .For(m => m.MaxWait, () => "00:02:00");
    }

    private ModelBuilder<KeyVaultSettingsConfig> InitializeKeyVaultSettingsConfigBuilder()
    {
        return new ModelBuilder<KeyVaultSettingsConfig>()
            .For(m => m.AzureAdApp, azureAdAppConfigBuilder.Build)
            .For(m => m.Connections, () => new List<ConnectionConfig> { connectionsConfigBuilder.Build() })
            .For(m => m.ConfigProvider, configProviderConfigBuilder.Build)
            .For(m => m.Retry, retryConfigBuilder.Build);
    }

    private static IConfiguration InitializeInMemoryConfiguration(KeyVaultSettingsConfig keyVaultSettingsConfig)
    {
        var inMemoryConfigurationSettings = new Dictionary<string, string?>();

        if (keyVaultSettingsConfig.AzureAdApp != null)
        {
            inMemoryConfigurationSettings.Add($"{KeyVaultSettingsProvider.KeyVaultSettingsKey}:{nameof(AzureAdApp)}:{nameof(AzureAdApp.AzureAdAppId)}", keyVaultSettingsConfig.AzureAdApp?.AzureAdAppId);
            inMemoryConfigurationSettings.Add($"{KeyVaultSettingsProvider.KeyVaultSettingsKey}:{nameof(AzureAdApp)}:{nameof(AzureAdApp.TenantId)}", keyVaultSettingsConfig.AzureAdApp?.TenantId);
            inMemoryConfigurationSettings.Add($"{KeyVaultSettingsProvider.KeyVaultSettingsKey}:{nameof(AzureAdApp)}:{nameof(AzureAdApp.ClientCertificateThumbprint)}", keyVaultSettingsConfig.AzureAdApp?.ClientCertificateThumbprint);
        }

        if (keyVaultSettingsConfig.Connections != null)
        {
            inMemoryConfigurationSettings.Add($"{KeyVaultSettingsProvider.KeyVaultSettingsKey}:{nameof(KeyVaultSettingsConfig.Connections)}:[0]:{nameof(Connection.Name)}", keyVaultSettingsConfig.Connections?.First()?.Name);
            inMemoryConfigurationSettings.Add($"{KeyVaultSettingsProvider.KeyVaultSettingsKey}:{nameof(KeyVaultSettingsConfig.Connections)}:[0]:{nameof(Connection.ServiceEndpoint)}", keyVaultSettingsConfig.Connections?.First()?.ServiceEndpoint);
            inMemoryConfigurationSettings.Add($"{KeyVaultSettingsProvider.KeyVaultSettingsKey}:{nameof(KeyVaultSettingsConfig.Connections)}:[0]:{nameof(Connection.Enabled)}", keyVaultSettingsConfig.Connections?.First()?.Enabled?.ToString());
        }

        if (keyVaultSettingsConfig.ConfigProvider != null)
        {

            inMemoryConfigurationSettings.Add($"{KeyVaultSettingsProvider.KeyVaultSettingsKey}:{nameof(KeyVaultSettingsConfig.ConfigProvider)}:{nameof(ConfigProvider.ReloadInterval)}", keyVaultSettingsConfig.ConfigProvider?.ReloadInterval?.ToString());
            inMemoryConfigurationSettings.Add($"{KeyVaultSettingsProvider.KeyVaultSettingsKey}:{nameof(KeyVaultSettingsConfig.ConfigProvider)}:{nameof(ConfigProvider.Enabled)}", keyVaultSettingsConfig.ConfigProvider?.Enabled?.ToString());
        }

        if (keyVaultSettingsConfig.Retry != null)
        {
            inMemoryConfigurationSettings.Add($"{KeyVaultSettingsProvider.KeyVaultSettingsKey}:{nameof(KeyVaultSettingsConfig.Retry)}:{nameof(Retry.MaxDelay)}", keyVaultSettingsConfig.Retry?.MaxDelay?.ToString());
            inMemoryConfigurationSettings.Add($"{KeyVaultSettingsProvider.KeyVaultSettingsKey}:{nameof(KeyVaultSettingsConfig.Retry)}:{nameof(Retry.SeedDelay)}", keyVaultSettingsConfig.Retry?.SeedDelay?.ToString());
            inMemoryConfigurationSettings.Add($"{KeyVaultSettingsProvider.KeyVaultSettingsKey}:{nameof(KeyVaultSettingsConfig.Retry)}:{nameof(Retry.SeedMultiplier)}", keyVaultSettingsConfig.Retry?.SeedMultiplier?.ToString());
            inMemoryConfigurationSettings.Add($"{KeyVaultSettingsProvider.KeyVaultSettingsKey}:{nameof(KeyVaultSettingsConfig.Retry)}:{nameof(Retry.MaxWait)}", keyVaultSettingsConfig.Retry?.MaxWait?.ToString());
            inMemoryConfigurationSettings.Add($"{KeyVaultSettingsProvider.KeyVaultSettingsKey}:{nameof(KeyVaultSettingsConfig.Retry)}:{nameof(Retry.MaxRetries)}", keyVaultSettingsConfig.Retry?.MaxRetries?.ToString());
        };

        var configurationBuilder = new ConfigurationBuilder();
        return configurationBuilder
            .AddInMemoryCollection(inMemoryConfigurationSettings)
            .Build();
    }

    [TestMethod]
    [TestCategory("Class Test")]
    public void GetKeyVaultSettings_WhenAllSettingsArePresentAndValid_ShouldReturnKeyVaultSettings()
    {
        // Arrange
        var expectedKeyVaultSettings = keyVaultSettingsConfigBuilder.Build();

        var configuration = InitializeInMemoryConfiguration(expectedKeyVaultSettings);

        // Act
        var actualKeyVaultSettings = KeyVaultSettingsProvider.GetKeyVaultSettings(configuration);

        // Assert
        AssertKeyVaultSettingsAreEqual(expectedKeyVaultSettings, actualKeyVaultSettings);
    }

    [TestMethod]
    [TestCategory("Class Test")]
    public void GetKeyVaultSettings_WhenNoSettingsArePresentForKeyVault_ShouldReturnKeyVaultSettingsWithNullSubProperties()
    {
        // Arrange
        var expectedKeyVaultSettings = new KeyVaultSettingsConfig(default!, default!, default!, default!);

        var configuration = InitializeInMemoryConfiguration(expectedKeyVaultSettings);

        // Act
        try
        {
            var actualKeyVaultSettings = KeyVaultSettingsProvider.GetKeyVaultSettings(configuration);
            Assert.Fail($"We were expecting an Exception of type {nameof(ConfigurationSettingMissingException)}, to be thrown, but no Exception was thrown");
        }
        catch (ConfigurationSettingMissingException e)
        {
            // Assert
            EnsureExceptionMessageContainsAzureAdAppRelatedMessages(e);
            EnsureExceptionMessageContainsConnectionsRelatedMessages(e);
            EnsureExceptionMessageContainsConfigProviderRelatedMessages(e);
            EnsureExceptionMessageContainsRetryRelatedMessages(e);
        }
    }

    [TestMethod]
    [TestCategory("Class Test")]
    public void GetKeyVaultSettings_WhenAzureAdAppSettingsAreNotPresent_ShouldThrow()
    {
        // Arrange
        var expectedKeyVaultSettings = keyVaultSettingsConfigBuilder
            .Set(m => m.AzureAdApp, null)
            .Build();

        var configuration = InitializeInMemoryConfiguration(expectedKeyVaultSettings);

        // Act
        try
        {
            var actualKeyVaultSettings = KeyVaultSettingsProvider.GetKeyVaultSettings(configuration);
            Assert.Fail($"We were expecting an Exception of type {nameof(ConfigurationSettingMissingException)}, to be thrown, but no Exception was thrown");
        }
        catch (ConfigurationSettingMissingException e)
        {
            // Assert
            EnsureExceptionMessageContainsAzureAdAppRelatedMessages(e);
        }
    }

    [TestMethod]
    [TestCategory("Class Test")]
    public void GetKeyVaultSettings_WhenConnectionsSettingsAreNotPresent_ShouldThrow()
    {
        // Arrange
        var expectedKeyVaultSettings = keyVaultSettingsConfigBuilder
            .Set(m => m.Connections, null)
            .Build();

        var configuration = InitializeInMemoryConfiguration(expectedKeyVaultSettings);

        // Act
        try
        {
            var actualKeyVaultSettings = KeyVaultSettingsProvider.GetKeyVaultSettings(configuration);
            Assert.Fail($"We were expecting an Exception of type {nameof(ConfigurationSettingMissingException)}, to be thrown, but no Exception was thrown");
        }
        catch (ConfigurationSettingMissingException e)
        {
            // Assert
            EnsureExceptionMessageContainsConnectionsRelatedMessages(e);
        }
    }

    [TestMethod]
    [TestCategory("Class Test")]
    public void GetKeyVaultSettings_WhenConfigProviderSettingsAreNotPresent_ShouldThrow()
    {
        // Arrange
        var expectedKeyVaultSettings = keyVaultSettingsConfigBuilder
            .Set(m => m.ConfigProvider, null)
            .Build();

        var configuration = InitializeInMemoryConfiguration(expectedKeyVaultSettings);

        // Act
        try
        {
            var actualKeyVaultSettings = KeyVaultSettingsProvider.GetKeyVaultSettings(configuration);
            Assert.Fail($"We were expecting an Exception of type {nameof(ConfigurationSettingMissingException)}, to be thrown, but no Exception was thrown");
        }
        catch (ConfigurationSettingMissingException e)
        {
            // Assert
            EnsureExceptionMessageContainsConfigProviderRelatedMessages(e);
        }
    }

    [TestMethod]
    [TestCategory("Class Test")]
    public void GetKeyVaultSettings_WhenRetrySettingsAreNotPresent_ShouldThrow()
    {
        // Arrange
        var expectedKeyVaultSettings = keyVaultSettingsConfigBuilder
            .Set(m => m.Retry, null)
            .Build();

        var configuration = InitializeInMemoryConfiguration(expectedKeyVaultSettings);

        // Act
        try
        {
            var actualKeyVaultSettings = KeyVaultSettingsProvider.GetKeyVaultSettings(configuration);
            Assert.Fail($"We were expecting an Exception of type {nameof(ConfigurationSettingMissingException)}, to be thrown, but no Exception was thrown");
        }
        catch (ConfigurationSettingMissingException e)
        {
            // Assert
            EnsureExceptionMessageContainsRetryRelatedMessages(e);
        }
    }

    [TestMethod]
    [TestCategory("Class Test")]
    public void GetKeyVaultSettings_WhenRetryMaxDelayIsNull_ShouldThrow()
    {
        // Arrange
        retryConfigBuilder
            .Set(m => m.MaxDelay, null);
        var sourceKeyVaultSettings = keyVaultSettingsConfigBuilder.Build();

        var configuration = InitializeInMemoryConfiguration(sourceKeyVaultSettings);

        try
        {
            // Act
            var actualKeyVaultSettings = KeyVaultSettingsProvider.GetKeyVaultSettings(configuration);
            Assert.Fail($"We were expecting an Exception of type {nameof(ConfigurationSettingMissingException)}, to be thrown, but no Exception was thrown");
        }
        catch (ConfigurationSettingMissingException e)
        {
            // Assert
            AssertEx.EnsureExceptionMessageContains(e, $"{nameof(Retry)}.{nameof(Retry.MaxDelay)}", "must be a valid TimeSpan", "can not be null");
        }
    }

    [TestMethod]
    [TestCategory("Class Test")]
    public void GetKeyVaultSettings_WhenRetryMaxDelayIsEmptyString_ShouldThrow()
    {
        // Arrange
        retryConfigBuilder
            .Set(m => m.MaxDelay, string.Empty);
        var sourceKeyVaultSettings = keyVaultSettingsConfigBuilder.Build();

        var configuration = InitializeInMemoryConfiguration(sourceKeyVaultSettings);

        try
        {
            // Act
            var actualKeyVaultSettings = KeyVaultSettingsProvider.GetKeyVaultSettings(configuration);
            Assert.Fail($"We were expecting an Exception of type {nameof(ConfigurationSettingMissingException)}, to be thrown, but no Exception was thrown");
        }
        catch (ConfigurationSettingMissingException e)
        {
            // Assert
            AssertEx.EnsureExceptionMessageContains(e, nameof(Retry.MaxDelay), "must be a valid TimeSpan", "can not be Empty");
        }
    }

    [TestMethod]
    [TestCategory("Class Test")]
    public void GetKeyVaultSettings_WhenRetryMaxDelayIsWhitespaces_ShouldThrow()
    {
        // Arrange
        retryConfigBuilder
            .Set(m => m.MaxDelay, "    ");
        var sourceKeyVaultSettings = keyVaultSettingsConfigBuilder.Build();

        var configuration = InitializeInMemoryConfiguration(sourceKeyVaultSettings);

        try
        {
            // Act
            var actualKeyVaultSettings = KeyVaultSettingsProvider.GetKeyVaultSettings(configuration);
            Assert.Fail($"We were expecting an Exception of type {nameof(ConfigurationSettingMissingException)}, to be thrown, but no Exception was thrown");
        }
        catch (ConfigurationSettingMissingException e)
        {
            // Assert
            AssertEx.EnsureExceptionMessageContains(e, nameof(Retry.MaxDelay), "must be a valid TimeSpan", "can not be Whitespaces");
        }
    }

    [TestMethod]
    [TestCategory("Class Test")]
    public void GetKeyVaultSettings_WhenRetryMaxDelayIsNotAValidTimeStampString_ShouldThrow()
    {
        // Arrange
        retryConfigBuilder
            .Set(m => m.MaxDelay, "00:00 min.");
        var sourceKeyVaultSettings = keyVaultSettingsConfigBuilder.Build();

        var configuration = InitializeInMemoryConfiguration(sourceKeyVaultSettings);

        try
        {
            // Act
            var actualKeyVaultSettings = KeyVaultSettingsProvider.GetKeyVaultSettings(configuration);
            Assert.Fail($"We were expecting an Exception of type {nameof(ConfigurationSettingMissingException)}, to be thrown, but no Exception was thrown");
        }
        catch (ConfigurationSettingMissingException e)
        {
            // Assert
            AssertEx.EnsureExceptionMessageContains(e, nameof(Retry.MaxDelay), "must be a valid TimeSpan", "is not a valid value for a TimeSpan");
        }
    }

    private static void EnsureExceptionMessageContainsAzureAdAppRelatedMessages(Exception exception)
    {
        AssertEx.EnsureExceptionMessageContains(exception, $"{nameof(AzureAdApp)}.{nameof(AzureAdApp.TenantId)}", "is missing and must be present");
        AssertEx.EnsureExceptionMessageContains(exception, $"{nameof(AzureAdApp)}.{nameof(AzureAdApp.AzureAdAppId)}", "is missing and must be present");
        AssertEx.EnsureExceptionMessageContains(exception, $"{nameof(AzureAdApp)}.{nameof(AzureAdApp.ClientCertificateThumbprint)}", "is missing and must be present");
    }

    private static void EnsureExceptionMessageContainsConnectionsRelatedMessages(Exception exception)
    {
        AssertEx.EnsureExceptionMessageContains(exception, $"{nameof(Connection)}s.{nameof(Connection.Name)}", "is missing and must be present");
        AssertEx.EnsureExceptionMessageContains(exception, $"{nameof(Connection)}s.{nameof(Connection.ServiceEndpoint)}", "is missing and must be present");
        AssertEx.EnsureExceptionMessageContains(exception, $"{nameof(Connection)}s.{nameof(Connection.Enabled)}", "is missing and must be present");
    }

    private static void EnsureExceptionMessageContainsConfigProviderRelatedMessages(Exception exception)
    {
        AssertEx.EnsureExceptionMessageContains(exception, $"{nameof(ConfigProvider)}.{nameof(ConfigProvider.Enabled)}", "is missing and must be present");
        AssertEx.EnsureExceptionMessageContains(exception, $"{nameof(ConfigProvider)}.{nameof(ConfigProvider.ReloadInterval)}", "is missing and must be present");
    }

    private static void EnsureExceptionMessageContainsRetryRelatedMessages(Exception exception)
    {
        AssertEx.EnsureExceptionMessageContains(exception, $"{nameof(Retry)}.{nameof(Retry.MaxWait)}", "is missing and must be present");
        AssertEx.EnsureExceptionMessageContains(exception, $"{nameof(Retry)}.{nameof(Retry.MaxRetries)}", "is missing and must be present");
        AssertEx.EnsureExceptionMessageContains(exception, $"{nameof(Retry)}.{nameof(Retry.MaxDelay)}", "is missing and must be present");
        AssertEx.EnsureExceptionMessageContains(exception, $"{nameof(Retry)}.{nameof(Retry.SeedDelay)}", "is missing and must be present");
        AssertEx.EnsureExceptionMessageContains(exception, $"{nameof(Retry)}.{nameof(Retry.SeedMultiplier)}", "is missing and must be present");
    }

    private static void AssertKeyVaultSettingsAreEqual(KeyVaultSettings expectedKeyVaultSettings, KeyVaultSettings actualKeyVaultSettings)
    {
        var errorMessages = new StringBuilder();
        errorMessages.AppendLineIfNotNull(DetermineIfAzureAdAppsAreEqual(expectedKeyVaultSettings.AzureAdApp, actualKeyVaultSettings.AzureAdApp));
        errorMessages.AppendLineIfNotNull(DetermineIfConfigProvidersAreEqual(expectedKeyVaultSettings.ConfigProvider, actualKeyVaultSettings.ConfigProvider));
        errorMessages.AppendLineIfNotNull(DetermineIfConnectionsAreEqual(expectedKeyVaultSettings.Connections, actualKeyVaultSettings.Connections));
        errorMessages.AppendLineIfNotNull(DetermineIfRetrysAreEqual(expectedKeyVaultSettings.Retry, actualKeyVaultSettings.Retry));

        if (errorMessages.Length > 0)
        {
            throw new AssertFailedException(errorMessages.ToString());
        }
    }

    private static string? DetermineIfAzureAdAppsAreEqual([NotNull] AzureAdApp expectedAzureAdApp, [NotNull] AzureAdApp actualAzureAdApp)
    {
        if (expectedAzureAdApp == actualAzureAdApp)
        {
            return null;
        }

        var errorMessages = new StringBuilder();
        if (expectedAzureAdApp.AzureAdAppId != actualAzureAdApp.AzureAdAppId)
        {
            errorMessages.AppendLine($"Expected {nameof(AzureAdApp)}.{nameof(AzureAdApp.AzureAdAppId)} is: `{expectedAzureAdApp.AzureAdAppId}`. Actual {nameof(AzureAdApp)}.{nameof(AzureAdApp.AzureAdAppId)} is: `{actualAzureAdApp.AzureAdAppId}`. ");
        }

        if (expectedAzureAdApp.ClientCertificateThumbprint != actualAzureAdApp.ClientCertificateThumbprint)
        {
            errorMessages.AppendLine($"Expected {nameof(AzureAdApp)}.{nameof(AzureAdApp.ClientCertificateThumbprint)} is: `{expectedAzureAdApp.ClientCertificateThumbprint}`. Actual {nameof(AzureAdApp)}.{nameof(AzureAdApp.ClientCertificateThumbprint)} is: `{actualAzureAdApp.ClientCertificateThumbprint}`. ");
        }

        if (expectedAzureAdApp.TenantId != actualAzureAdApp.TenantId)
        {
            errorMessages.AppendLine($"Expected {nameof(AzureAdApp)}.{nameof(AzureAdApp.TenantId)} is: `{expectedAzureAdApp.TenantId}`. Actual {nameof(AzureAdApp)}.{nameof(AzureAdApp.TenantId)} is: `{actualAzureAdApp.TenantId}`. ");
        }

        return errorMessages.ToString();
    }

    private static string? DetermineIfConfigProvidersAreEqual(ConfigProvider expectedConfigProvider, ConfigProvider actualConfigProvider)
    {
        if (expectedConfigProvider == actualConfigProvider)
        {
            return null;
        }

        var errorMessages = new StringBuilder();
        if (expectedConfigProvider.Enabled != actualConfigProvider.Enabled)
        {
            errorMessages.AppendLine($"Expected {nameof(ConfigProvider)}.{nameof(ConfigProvider.Enabled)} is: `{expectedConfigProvider.Enabled}`. Actual {nameof(ConfigProvider)}.{nameof(ConfigProvider.Enabled)} is: `{actualConfigProvider.Enabled}`. ");
        }

        if (expectedConfigProvider.ReloadInterval != actualConfigProvider.ReloadInterval)
        {
            errorMessages.AppendLine($"Expected {nameof(ConfigProvider)}.{nameof(ConfigProvider.ReloadInterval)} is: `{expectedConfigProvider.ReloadInterval}`. Actual {nameof(ConfigProvider)}.{nameof(ConfigProvider.ReloadInterval)} is: `{actualConfigProvider.ReloadInterval}`. ");
        }

        return errorMessages.ToString();
    }

    private static string? DetermineIfConnectionsAreEqual(IEnumerable<Connection> expectedConnections, IEnumerable<Connection> actualConnections)
    {
        var connectionInExpectedNotInActual = expectedConnections.Except(actualConnections);
        var connectionInActualNotInExpected = actualConnections.Except(expectedConnections);

        var errorMessages = new StringBuilder();

        if (connectionInExpectedNotInActual.Any())
        {
            foreach (var expectedConnectionNotInActual in connectionInExpectedNotInActual)
            {
                errorMessages.AppendLine($"{nameof(Connection)} in Expected, was not found in Actual.");
                errorMessages.AppendLine($"Expected {nameof(Connection)} properties and their values are:");
                errorMessages.AppendLine($"{nameof(Connection)}.{nameof(Connection.Enabled)} is: `{expectedConnectionNotInActual.Enabled}`");
                errorMessages.AppendLine($"{nameof(Connection)}.{nameof(Connection.ServiceEndpoint)} is: `{expectedConnectionNotInActual.ServiceEndpoint}`");
            }
        }

        if (connectionInActualNotInExpected.Any())
        {
            foreach (var actualConnectionNotInExpected in connectionInActualNotInExpected)
            {
                errorMessages.AppendLine($"{nameof(Connection)} in Actual, was not found in Expected.");
                errorMessages.AppendLine($"Actual {nameof(Connection)} properties and their values are:");
                errorMessages.AppendLine($"{nameof(Connection)}.{nameof(Connection.Enabled)} is: `{actualConnectionNotInExpected.Enabled}`");
                errorMessages.AppendLine($"{nameof(Connection)}.{nameof(Connection.ServiceEndpoint)} is: `{actualConnectionNotInExpected.ServiceEndpoint}`");
            }
        }

        return null;
    }

    private static string? DetermineIfRetrysAreEqual(Retry expectedRetry, Retry actualRetry)
    {
        if (expectedRetry == actualRetry)
        {
            return null;
        }

        var errorMessages = new StringBuilder();
        if (expectedRetry.MaxDelay != actualRetry.MaxDelay)
        {
            errorMessages.AppendLine($"Expected {nameof(Retry)}.{nameof(Retry.MaxDelay)} is: `{expectedRetry.MaxDelay}`. Actual {nameof(Retry)}.{nameof(Retry.MaxDelay)} is: `{actualRetry.MaxDelay}`. ");
        }

        if (expectedRetry.SeedDelay != actualRetry.SeedDelay)
        {
            errorMessages.AppendLine($"Expected {nameof(Retry)}.{nameof(Retry.SeedDelay)} is: `{expectedRetry.SeedDelay}`. Actual {nameof(Retry)}.{nameof(Retry.SeedDelay)} is: `{actualRetry.SeedDelay}`. ");
        }

        if (expectedRetry.MaxWait != actualRetry.MaxWait)
        {
            errorMessages.AppendLine($"Expected {nameof(Retry)}.{nameof(Retry.MaxWait)} is: `{expectedRetry.MaxWait}`. Actual {nameof(Retry)}.{nameof(Retry.MaxWait)} is: `{actualRetry.MaxWait}`. ");
        }

        if (expectedRetry.MaxRetries != actualRetry.MaxRetries)
        {
            errorMessages.AppendLine($"Expected {nameof(Retry)}.{nameof(Retry.MaxRetries)} is: `{expectedRetry.MaxRetries}`. Actual {nameof(Retry)}.{nameof(Retry.MaxRetries)} is: `{actualRetry.MaxRetries}`. ");
        }

        return errorMessages.ToString();
    }
}