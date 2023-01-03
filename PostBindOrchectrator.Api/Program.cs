using PostBindOrchestrator.Api;
using PostBindOrchestrator.DomainLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationInsightsTelemetryWorkerService();

builder.Services.AddSingleton<ServiceLocatorBase, ServiceLocator>(sp =>
{
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
    return new ServiceLocator(loggerFactory);
});

builder.Services.AddSingleton(sp =>
{
    var serviceLocator = sp.GetRequiredService<ServiceLocatorBase>();
    return new ApplicationLogger(serviceLocator.CreateLogger());
});

builder.Services.AddSingleton(sp =>
{
    var serviceLocator = sp.GetRequiredService<ServiceLocatorBase>();
    return new DomainFacade(serviceLocator);
});

builder.Services.AddScoped<CorrelationIdProvider>();
builder.Services.AddSingleton<RouteHandlerPostBind>();
builder.Services.AddSingleton<RouteHandlerRevertToQuote>();

builder.Services.AddHostedService<MessageBrokerWorker>();

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

public partial class Program { }