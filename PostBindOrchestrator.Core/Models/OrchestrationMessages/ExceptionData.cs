using Azure.Messaging.ServiceBus;

namespace PostBindOrchestrator.Core.Models.OrchestrationMessages;
public sealed class ExceptionData
{
    public string ExceptionType { get; }
    public string Message { get; }
    public string StackTrace { get; }
    public string? Reason { get; }
    public string? Category { get; }
    public ExceptionData? InnermostException { get; set; }

    public ExceptionData(string exceptionType, string message, string stackTrace, string? reason = default, string? category = default, ExceptionData? innermostException = default)
    {
        ExceptionType = exceptionType;
        Message = message;
        StackTrace = stackTrace;
        Reason = reason;
        Category = category;
        InnermostException = innermostException;
    }

    public static explicit operator ExceptionData(Exception exception)
    {
        var (reason, category) = GetExceptionReasonAndCategory(exception);

        var exceptionData = new ExceptionData(exception.GetType().Name, exception.Message, exception.StackTrace!, reason, category);
        
        var innermostException = exception.GetBaseException();
        if (innermostException is not null && innermostException != exception)
        {
            var (innerReason, innerCategory) = GetExceptionReasonAndCategory(innermostException);
            exceptionData.InnermostException = new ExceptionData(innermostException.GetType().Name, innermostException.Message, innermostException.StackTrace!, innerReason, innerCategory);
        }
        
        return exceptionData;
    }

    public static (string? reason, string category) GetExceptionReasonAndCategory(Exception exception)
    {
        return (exception is PostBindOrchestratorBaseException baseException)
                ? (baseException.Reason, baseException.Category)
                : (exception is ServiceBusException serviceBusException)
                    ? (serviceBusException.Reason.ToString(), "Technical")
                    : (null, "Technical");
    }
}
