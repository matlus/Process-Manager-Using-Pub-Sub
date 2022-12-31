namespace PostBindOrchestrator.Api;

public static class EndpointRouteBuilderExtensions
{
    public static void MapRoutesForRouteHandlers(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var postBindHandler = endpointRouteBuilder.ServiceProvider.GetRequiredService<RouteHandlerPostBind>();
        postBindHandler.MapRoutes(endpointRouteBuilder);

        var revertToQuoteRouteHandler = endpointRouteBuilder.ServiceProvider.GetRequiredService<RouteHandlerRevertToQuote>();
        revertToQuoteRouteHandler.MapRoutes(endpointRouteBuilder);
    }
}
