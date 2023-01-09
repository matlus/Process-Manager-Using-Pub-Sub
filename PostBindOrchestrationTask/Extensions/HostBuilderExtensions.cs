using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace PostBindOrchestrationTask;

internal static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureAppSettingsJson(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureAppConfiguration(configurationBuilder =>
        {
            configurationBuilder
                    .SetBasePath(Directory.GetCurrentDirectory() + @"\ApplicationSettings")
                    .AddJsonFile("appsettings.json");

            var aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (aspNetCoreEnvironment != null)
            {
                configurationBuilder.AddJsonFile($"appsettings.{aspNetCoreEnvironment}.json");
            }
        });
    }

    public static IConfigurationBuilder ConfigureAppSettingsJson(this IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
        .SetBasePath(Directory.GetCurrentDirectory() + @"\ApplicationSettings")
        .AddJsonFile("appsettings.json");

        var aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (aspNetCoreEnvironment != null)
        {
            configurationBuilder.AddJsonFile($"appsettings.{aspNetCoreEnvironment}.json");
        }

        return configurationBuilder;
    }
}
