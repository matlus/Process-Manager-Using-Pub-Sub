using PostBindOrchestrator.Api;
using PostBindOrchestrator.DomainLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ServiceLocatorBase, ServiceLocator>();

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

builder.Services.AddHostedService<MessageBrokerWorker>();

builder.Services.AddScoped<CorrelationIdProvider>();

var app = builder.Build();


// Configure the HTTP request pipeline.

app.Use(async (context, next) =>
{
    var correlationIdHeaders = context.Request.Headers["X-Correlation-Id"];

    if (correlationIdHeaders.Any())
    {
        var corrleationId = correlationIdHeaders.First()!;
        context.Items["CorrelationId"] = corrleationId;
        var sp = context.RequestServices;
        var correlationIdProvider = sp.GetRequiredService<CorrelationIdProvider>();
        correlationIdProvider.CorrelationId = corrleationId;
    }

    await next(context);
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var domainFacade = app.Services.GetRequiredService<DomainFacade>();
var postBindHandler = new PostBindRouteHandler(domainFacade);

app.MapGet("/processpostbind/{policyNumber}", postBindHandler.ProcessPostBind)
    .WithName("ProcessPostBind");

app.Run();

public sealed class CorrelationIdProvider
{
    public string? CorrelationId { get; set; }
}