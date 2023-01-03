using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

namespace PostBindOrchestrator.DomainLayer;

internal sealed class LoggerProvider : IDisposable
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ITelemetryChannel telemetryChannel;
    private readonly ConcurrentDictionary<string, ILogger> loggersDictionary = new();
    private readonly string eventLogName;

    private bool disposed;

    public LoggerProvider(string eventLogName, Func<IConfiguration> loggingConfigurationDelegate, string? appInsightsConnectionString = null)
    {
        this.eventLogName = eventLogName;
        telemetryChannel = new InMemoryChannel();

        loggerFactory = new LoggerFactory();
        loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
            .ClearProviders()
            .AddConfiguration(loggingConfigurationDelegate())
            .AddConsole()
            .AddEventSourceLogger()
            .AddApplicationInsights(
                configureTelemetryConfiguration: config =>
                {
                    config.ConnectionString = appInsightsConnectionString;
                    config.TelemetryChannel = telemetryChannel;
                },
                configureApplicationInsightsLoggerOptions: options => { });

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                builder.AddEventLog(new EventLogSettings { LogName = eventLogName, SourceName = eventLogName });
            }
        });
    }

    public ILogger CreateLogger()
    {
        return loggersDictionary.GetOrAdd(eventLogName, loggerFactory.CreateLogger(eventLogName));
    }

    private void Dispose(bool disposing)
    {
        if (disposing && !disposed)
        {
            telemetryChannel.Flush();
            telemetryChannel.Dispose();
            loggerFactory.Dispose();
            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
