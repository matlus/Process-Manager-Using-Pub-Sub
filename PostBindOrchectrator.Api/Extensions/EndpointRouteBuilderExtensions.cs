namespace PostBindOrchestrator.Api.ApplicationBuilderExtensions;

public static class EndpointRouteBuilderExtensions
{
    public static void MapRoutesForRouteHandlers(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var postBindHandler = endpointRouteBuilder.ServiceProvider.GetRequiredService<PostBindRouteHandler>();
        postBindHandler.MapRoutes(endpointRouteBuilder);

        var revertToQuoteRouteHandler = endpointRouteBuilder.ServiceProvider.GetRequiredService<RevertToQuoteRouteHandler>();
        revertToQuoteRouteHandler.MapRoutes(endpointRouteBuilder);
    }
}
