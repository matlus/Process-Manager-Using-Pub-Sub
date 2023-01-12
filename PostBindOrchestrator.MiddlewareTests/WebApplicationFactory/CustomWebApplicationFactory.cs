using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PostBindOrchestrator.Api;

namespace PostBindOrchestrator.MiddlewareTests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services => ReplaceService<RouteHandlerPostBind, RouteHandlerPostBindSpy>(services));
        builder.ConfigureServices(services => ReplaceService<RouteHandlerRevertToQuote, RouteHandlerRevertToQuoteSpy>(services));

        var loggerProviderSpy = new LoggerProviderSpy();

        builder.ConfigureServices(services => services.AddSingleton(serviceProvider => loggerProviderSpy));
        builder.ConfigureServices(service => service.AddLogging(loggerBuilder =>
            {
                loggerBuilder.ClearProviders();
                loggerBuilder.AddProvider(loggerProviderSpy);
            }));
    }

    private static void ReplaceService<TBase, TImplementation>(IServiceCollection serviceCollection) where TBase : class
                                                                                                     where TImplementation : class, TBase
    {
        var serviceDescriptor = serviceCollection.Single(d => d.ServiceType == typeof(TBase));
        serviceCollection.Remove(serviceDescriptor);
        serviceCollection.AddSingleton<TBase, TImplementation>();
    }
}
