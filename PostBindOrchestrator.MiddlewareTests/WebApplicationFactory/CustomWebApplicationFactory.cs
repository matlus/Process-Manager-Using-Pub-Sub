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
        builder.ConfigureServices(services => ReplaceService<RouteHandlerPostBind, PostBindRouteHandlerSpy>(services));
    }

    private static void ReplaceService<TBase, TImplementation>(IServiceCollection serviceCollection) where TBase : class
                                                                                                     where TImplementation : class, TBase
    {
        var serviceDescriptor = serviceCollection.Single(d => d.ServiceType == typeof(TBase));
        serviceCollection.Remove(serviceDescriptor);
        serviceCollection.AddSingleton<TBase, TImplementation>();
    }
}
