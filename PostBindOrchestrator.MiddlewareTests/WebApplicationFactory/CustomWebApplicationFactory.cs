using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PostBindOrchestrator.Api;

namespace PostBindOrchestrator.MiddlewareTests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _ = builder.ConfigureServices(services => ReplaceService<RouteHandlerPostBind, RouteHandlerPostBindSpy>(services));
        _ = builder.ConfigureServices(services => ReplaceService<RouteHandlerRevertToQuote, RouteHandlerRevertToQuoteSpy>(services));
    }

    private static void ReplaceService<TBase, TImplementation>(IServiceCollection serviceCollection) where TBase : class
                                                                                                     where TImplementation : class, TBase
    {
        var serviceDescriptor = serviceCollection.Single(d => d.ServiceType == typeof(TBase));
        _ = serviceCollection.Remove(serviceDescriptor);
        _ = serviceCollection.AddSingleton<TBase, TImplementation>();
    }
}
