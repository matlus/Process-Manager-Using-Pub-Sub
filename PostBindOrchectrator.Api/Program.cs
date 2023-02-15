using Microsoft.ApplicationInsights.Extensibility;
using PostBindOrchestrator.Api;
using PostBindOrchestrator.Api.Providers;
using PostBindOrchestrator.Core;
using PostBindOrchestrator.DomainLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddHttpContextAccessor()
    .AddApplicationInsightsTelemetryWorkerService()
    .AddSingleton<ServiceLocatorBase, ServiceLocator>()
    .AddSingleton(sp =>
        {
            var serviceLocator = sp.GetRequiredService<ServiceLocatorBase>();
            return new ApplicationLogger(serviceLocator.CreateLogger());
        })
    .AddSingleton<ITelemetryInitializer, RoleNameTelemetryInitializer>(serviceProvider =>
    {
        var serviceLocator = serviceProvider.GetRequiredService<ServiceLocatorBase>();
        var configurationProvider = serviceLocator.CreateConfigurationProvider();
        var roleName = configurationProvider.GetRoleName();
        return new RoleNameTelemetryInitializer(roleName);
    })
    .AddSingleton<DomainFacade>()
    .AddScoped<CorrelationIdProvider>()
    .AddSingleton<RouteHandlerPostBind>()
    .AddSingleton<RouteHandlerRevertToQuote>()
    .AddHostedService<MessageBrokerWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandling();
app.UseCorrelationIdInitializer();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapRoutesForRouteHandlers();

app.Run();

internal sealed partial class Program { }
