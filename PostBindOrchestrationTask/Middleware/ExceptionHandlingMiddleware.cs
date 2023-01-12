using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace PostBindOrchestrationTask;

public sealed class ExceptionHandlingMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            while (e.InnerException is not null)
            {
                e = e.InnerException;
            }

            if (context.BindingContext.BindingData is not null)
            {
                var sb = new StringBuilder();
                var values = new List<object>();

                sb.Append("Exception caught in Exception Handling Middleware. An Exception of Type:{ExceptionType} with Exception Category: {ExceptionCategory} occured, with Message: {Message}");
                values.Add(e.GetType().Name);
                values.Add("Technical");
                values.Add(e.Message);

                foreach (var item in context.BindingContext.BindingData)
                {
                    if (item.Value is not null)
                    {
                        sb.Append($"{{{item.Key}}}, ");
                        values.Add(item.Value);
                    }
                }

                _logger.LogError(e, sb.ToString(), values.ToArray());
            }
            else
            {
                _logger.LogError(e, "Error Processing Invocation. Exception caught in Exception Handling Middleware. ExceptionCategory: {ExceptionCategory}", "Technical");
            }
        }
    }
}
