using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace PostBindOrchestrator.Core;

#pragma warning disable CA2254
[ExcludeFromCodeCoverage]
public sealed partial class ApplicationLogger
{
    public ILogger Logger { get; }

    public ApplicationLogger(ILogger logger) => Logger = logger;

    public void LogInformation(int eventId, string? message, params object?[] args)
    {
        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation(eventId, message, args);
        }
    }

    public void LogError<T1, T2, T3>(LogEvent logEvent, Exception exception, string? message, T1 param1, T2 param2, T3 param3)
    {
        if (Logger.IsEnabled(LogLevel.Error))
        {
            Logger.LogError((int)logEvent, exception, message, param1, param2, param3);
        }
    }

    public void LogError<T1, T2, T3, T4, T5>(LogEvent logEvent, Exception exception, string? message, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
    {
        if (Logger.IsEnabled(LogLevel.Error))
        {
            Logger.LogError((int)logEvent, exception, message, param1, param2, param3, param4, param5);
        }
    }

    public void LogError<T1, T2, T3, T4, T5, T6>(LogEvent logEvent, Exception exception, string? message, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6)
    {
        if (Logger.IsEnabled(LogLevel.Error))
        {
            Logger.LogError((int)logEvent, exception, message, param1, param2, param3, param4, param5, param6);
        }
    }

    public void LogError(LogEvent logEvent, Exception exception, string methodName, BrokerMessage brokerMessage)
    {
        if (Logger.IsEnabled(LogLevel.Error))
        {
            var logStateBrokerMessage = new LogStateBrokerMessage(logEvent, methodName, brokerMessage, exception);
            Logger.Log(LogLevel.Error, new EventId((int)logEvent, logEvent.ToString()), logStateBrokerMessage, exception, LogStateBrokerMessage.Format);
        }
    }

    public void LogError(LogEvent logEvent, Exception exception, string methodName, OrchestrationTaskMessage orchestrationTaskMessage)
    {
        if (Logger.IsEnabled(LogLevel.Error))
        {
            var logStateOrchestrationTaskMessage = new LogStateOrchestrationTaskMessage(logEvent, methodName, orchestrationTaskMessage, exception);
            Logger.Log(LogLevel.Error, new EventId((int)logEvent, logEvent.ToString()), logStateOrchestrationTaskMessage, exception, LogStateOrchestrationTaskMessage.Format);
        }
    }

    public void LogError(LogEvent logEvent, Exception exception, string methodName, OrchestrationReplyMessage orchestrationReplyMessage)
    {
        if (Logger.IsEnabled(LogLevel.Error))
        {
            var logStateOrchestrationReplyMessage = new LogStateOrchestrationReplyMessage(logEvent, methodName, orchestrationReplyMessage, exception);
            Logger.Log(LogLevel.Error, new EventId((int)logEvent, logEvent.ToString()), logStateOrchestrationReplyMessage, exception, LogStateOrchestrationReplyMessage.Format);
        }
    }
}
#pragma warning restore CA2254
