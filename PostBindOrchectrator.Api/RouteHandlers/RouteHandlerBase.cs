namespace PostBindOrchestrator.Api;

internal abstract class RouteHandlerBase
{
    public void MapRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        MapRoutesCore(endpointRouteBuilder);
    }

    protected static void MapPostEndpoint(IEndpointRouteBuilder endpointRouteBuilder, string pattern, string endpointName, Delegate handler)
    {
        endpointRouteBuilder.MapPost(pattern, handler)
        .WithName(endpointName)
        .WithTags(endpointName)
        .WithDisplayName(endpointName);
    }

    protected abstract void MapRoutesCore(IEndpointRouteBuilder endpointRouteBuilder);
}
