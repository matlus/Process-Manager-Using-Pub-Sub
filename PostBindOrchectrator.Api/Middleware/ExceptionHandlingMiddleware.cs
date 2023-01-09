using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.Api;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ApplicationLogger applicationLogger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ApplicationLogger applicationLogger)
    {
        this.next = next;
        this.applicationLogger = applicationLogger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception e)
        {
            await ExceptionToHttpTranslator.Translate(httpContext, e, applicationLogger);
        }
    }
}
