namespace PostBindOrchestrator.Api;

internal sealed class CorrelationIdInitializerMiddleware
{
    private readonly RequestDelegate next;

    public CorrelationIdInitializerMiddleware(RequestDelegate next) => this.next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.TryGetValue("X-Correlation-Id", out var stringValues) && stringValues.Count > 0)
        {
            var corrleationId = stringValues[0]!;
            httpContext.Items["CorrelationId"] = corrleationId;
        }

        await next(httpContext);
    }
}