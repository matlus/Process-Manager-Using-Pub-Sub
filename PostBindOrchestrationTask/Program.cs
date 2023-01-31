using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PostBindOrchestrationTask;
using PostBindOrchestrationTask.DomainLayer;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(functionsWorkerApplicationBuilder =>
    {
        functionsWorkerApplicationBuilder.UseMiddleware<ExceptionHandlingMiddleware>();
        functionsWorkerApplicationBuilder.Services.AddApplicationInsightsTelemetryWorkerService();
    })
    .ConfigureAppSettingsJson()
    .ConfigureAppConfiguration(configurationBuilder =>
    {
        var configurationRoot = configurationBuilder.Build();
        var connectionString = configurationRoot["ApplicationInsights:ConnectionString"];
        configurationBuilder.AddApplicationInsightsSettings(connectionString);

    })
    .ConfigureServices(serviceCollection => serviceCollection
        .AddSingleton<ServiceLocatorBase, ServiceLocator>()
        .AddSingleton<DomainFacade>())
    .Build();

host.Run();

internal sealed partial class Program { }