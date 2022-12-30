namespace PostBindOrchestrator.Api.ApplicationBuilderExtensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    public static IApplicationBuilder UseCorrelationIdInitializer(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<CorrelationIdInitializerMiddleware>();
    }
}