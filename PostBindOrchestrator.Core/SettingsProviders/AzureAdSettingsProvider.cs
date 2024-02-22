using System.Text;
using Microsoft.Extensions.Configuration;

namespace PostBindOrchestrator.Core;

public static class AzureAdSettingsProvider
{
    private const string azureAdSettingsKey = "AzureAd";
    private const string instancePropertyName = "Instance";
    private const string clientIdPropertyName = "ClientId";
    private const string clientSecretPropertyName = "ClientSecret";
    private const string domainPropertyName = "Domain";
    private const string tenantIdPropertyName = "TenantId";
    private const string audiencePropertyName = "Audience";

    /// <summary> 
    ///  
    /// </summary>
    /// <example>
    /// In the appsettings.json file, the AzureAdSettings section looks like the example code below.
    /// Valid values for the "MessageBrokerType" property are those of enum <see cref="MessageBrokerType"/>
    /// <code>
    ///   "AzureAd": {
    ///   "Instance": "https://login.microsoftonline.com/",
    ///   "ClientId": "20a613f8-8cc4-400f-8f3c-e3a8f00f0916",
    ///   "ClientSecret": "wtZBQ-qEmvOomY-ROQorB8mX5.SF6loADZ",
    ///   "Domain": "mycompany.onmicrosoft.com",
    ///   "TenantId": "4418d333-8764-439a-8586-0ce65633673d",
    ///   "Audience": "https://mycompany.onmicrosoft.com/8bbfe630-9150-4bdf-bd7b-e7105e0f8d80"
    ///   }
    /// </code>
    /// </example>
    /// <param name="configurationRoot">The <see cref="IConfiguration"/></param>
    /// <param name="retrieveConfigurationSettingValueOrNull">A Func that returns the configuration setting value or null if the value is missing, empty or white spaces</param>
    /// <returns>A validated AzureAdSettings instance</returns>
    public static AzureAdSettings GetAzureAdSettings(IConfiguration configuration)
    {
        var messageBrokerSettingsConfig = GetAzureAdSettingsPreValidated(configuration);
        Validate(messageBrokerSettingsConfig);
        return messageBrokerSettingsConfig;
    }

    public static AzureAdSettingsConfig GetAzureAdSettingsPreValidated(IConfiguration configuration)
    {
        var azureAdSettingsConfig = configuration.GetSection(azureAdSettingsKey).Get<AzureAdSettingsConfig>();
        return azureAdSettingsConfig ?? new AzureAdSettingsConfig();
    }

    private static void Validate(AzureAdSettingsConfig azureAdSettingsConfig)
    {
        var errorMessages = new StringBuilder();

        ValidateAzureAdSettingsConfig(errorMessages, azureAdSettingsConfig);

        if (errorMessages.Length is not 0)
        {
            throw new ConfigurationSettingMissingException(errorMessages.ToString());
        }
    }

    private static void ValidateAzureAdSettingsConfig(StringBuilder errorMessages, AzureAdSettingsConfig azureAdSettingsConfig)
    {
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{azureAdSettingsKey}Settings.{instancePropertyName}", azureAdSettingsConfig.Instance));
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{azureAdSettingsKey}Settings.{clientIdPropertyName}", azureAdSettingsConfig.ClientId));
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{azureAdSettingsKey}Settings.{clientSecretPropertyName}", azureAdSettingsConfig.ClientSecret));
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{azureAdSettingsKey}Settings.{domainPropertyName}", azureAdSettingsConfig.Domain));
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{azureAdSettingsKey}Settings.{tenantIdPropertyName}", azureAdSettingsConfig.TenantId));
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{azureAdSettingsKey}Settings.{audiencePropertyName}", azureAdSettingsConfig.Audience));
    }
}