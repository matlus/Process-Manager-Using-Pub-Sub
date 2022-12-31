using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using PostBindOrchestrator.Core;
using PostBindOrchestrator.DomainLayer;

namespace PostBindOrchestrator.Api;

public static class ExceptionToHttpTranslator
{
    public static async Task Translate(HttpContext httpContext, Exception exception, ApplicationLogger appLogger)
    {
        var httpResponse = httpContext.Response;
        httpResponse.Headers.Add("Exception-Type", exception.GetType().Name);

        if (exception is PostBindOrchestratorBaseException postBindOrchestratorBaseException)
        {
            httpContext.Features.Get<IHttpResponseFeature>()!.ReasonPhrase = postBindOrchestratorBaseException.Reason;
        }

        httpResponse.StatusCode = MapExceptionToStatusCode(exception);

        string? corrleationId = null;
        if (httpContext.Request.Headers.TryGetValue("X-Correlation-Id", out StringValues values) && values.Any())
        {
            corrleationId = values.First()!;
        }

        appLogger.LogError(
            logEvent: LogEvent.Middleware,
            exception,
            message: "{Location}: {Step}, {CorrelationId}, {StatusCode}, {ExceptionType} with: {ExceptionMessage}",
            nameof(Translate),
            "Middleware",
            corrleationId,
            httpResponse.StatusCode,
            exception.GetType().Name,
            exception.Message);

        await httpResponse.WriteAsync(exception.Message);
    }

    private static int MapExceptionToStatusCode(Exception exception)
    {
        //// If we have a "Not Found" type of exception, for example Policy Not Found
        //// we need to have a condition for that in order to return a 404 status code

        return exception is PostBindOrchestratorBusinessBaseException ? 400 : 500;
    }
}
