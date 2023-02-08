using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using PostBindOrchestrator.Api;
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
    .AddSingleton<DomainFacade>()
    .AddScoped<CorrelationIdProvider>()
    .AddSingleton<RouteHandlerPostBind>()
    .AddSingleton<RouteHandlerRevertToQuote>()
    .AddSingleton<ITelemetryInitializer, MyTelemetryInitializer>()
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

internal sealed class MyTelemetryInitializer : ITelemetryInitializer
{
    public void Initialize(ITelemetry telemetry)
    {
        telemetry.Context.Cloud.RoleName = "PostBindOrchestrator";
    }
}
