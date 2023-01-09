using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PostBindOrchestrator.Core.Models.OrchestrationMessages;
public sealed class ExceptionData
{
    public string ExceptionType { get; }
    public string Message { get; }
    public string StackTrace { get; }
    public string? Reason { get; }
    public string? Category { get; }

    public ExceptionData(string exceptionType, string message, string stackTrace, string? reason = default, string? category = default)
    {
        ExceptionType = exceptionType;
        Message = message;
        StackTrace = stackTrace;
        Reason = reason;
        Category = category;
    }

    public static explicit operator ExceptionData(Exception exception)
    {
        var (reason, category) = (exception is PostBindOrchestratorBaseException baseException)
            ? (baseException.Reason, baseException.Category)
            : (null, null);

        return new ExceptionData(exception.GetType().Name, exception.Message, exception.StackTrace!, reason, category);
    }
}
