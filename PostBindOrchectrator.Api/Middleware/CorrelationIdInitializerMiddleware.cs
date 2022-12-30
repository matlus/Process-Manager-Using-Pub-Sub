using Microsoft.Extensions.Primitives;

namespace PostBindOrchestrator.Api;

public sealed class CorrelationIdInitializerMiddleware
{
    private readonly RequestDelegate next;

    public CorrelationIdInitializerMiddleware(RequestDelegate next) => this.next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.TryGetValue("X-Correlation-Id", out StringValues values) && values.Any())
        {
            var corrleationId = values.First()!;
            httpContext.Items["CorrelationId"] = corrleationId;
        }

        await next(httpContext);
    }
}