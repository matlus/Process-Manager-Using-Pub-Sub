using System.Text;
using Microsoft.Extensions.Configuration;

namespace PostBindOrchestrator.Core;

public static class AzureAdSettingsProvider
{
    private const string azureAdSettingsKey = "AzureAd";

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
        var azureAdSettingsConfig = GetAzureAdSettingsUnValidated(configuration);
        Validate(azureAdSettingsConfig);
        return azureAdSettingsConfig;
    }

    public static AzureAdSettingsConfig GetAzureAdSettingsUnValidated(IConfiguration configuration)
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
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{azureAdSettingsKey}.{nameof(AzureAdSettings.Instance)}", azureAdSettingsConfig.Instance));
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{azureAdSettingsKey}.{nameof(AzureAdSettings.ClientId)}", azureAdSettingsConfig.ClientId));
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{azureAdSettingsKey}.{nameof(AzureAdSettings.ClientSecret)}", azureAdSettingsConfig.ClientSecret));
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{azureAdSettingsKey}.{nameof(AzureAdSettings.Domain)}", azureAdSettingsConfig.Domain));
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{azureAdSettingsKey}.{nameof(AzureAdSettings.TenantId)}", azureAdSettingsConfig.TenantId));
        errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"{azureAdSettingsKey}.{nameof(AzureAdSettings.Audience)}", azureAdSettingsConfig.Audience));
    }
}