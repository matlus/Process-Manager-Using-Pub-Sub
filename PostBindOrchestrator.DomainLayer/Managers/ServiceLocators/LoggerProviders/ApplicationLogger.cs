using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;

[ExcludeFromCodeCoverage]
public sealed partial class ApplicationLogger
{
    public ILogger Logger { get; }

    public ApplicationLogger(ILogger logger) => Logger = logger;

    public void LogInformation(int eventId, string? message, params object?[] args)
    {
        if (Logger.IsEnabled(LogLevel.Information))
        {
#pragma warning disable CA2254 // Template should be a static expression
            Logger.LogInformation(eventId, message, args);
#pragma warning restore CA2254 // Template should be a static expression
        }
    }

    public void LogError<T1, T2, T3>(LogEvent logEvent, Exception exception, string? message, T1 param1, T2 param2, T3 param3)
    {
        if (Logger.IsEnabled(LogLevel.Error))
        {
#pragma warning disable CA2254 // Template should be a static expression
            Logger.LogError((int)logEvent, exception, message, param1, param2, param3);
#pragma warning restore CA2254 // Template should be a static expression
        }
    }

    public void LogError<T1, T2, T3, T4, T5>(LogEvent logEvent, Exception exception, string? message, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
    {
        if (Logger.IsEnabled(LogLevel.Error))
        {
#pragma warning disable CA2254 // Template should be a static expression
            Logger.LogError((int)logEvent, exception, message, param1, param2, param3, param4, param5);
#pragma warning restore CA2254 // Template should be a static expression
        }
    }

    public void LogError<T1, T2, T3, T4, T5, T6>(LogEvent logEvent, Exception exception, string? message, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6)
    {
        if (Logger.IsEnabled(LogLevel.Error))
        {
#pragma warning disable CA2254 // Template should be a static expression
            Logger.LogError((int)logEvent, exception, message, param1, param2, param3, param4, param5, param6);
#pragma warning restore CA2254 // Template should be a static expression
        }
    }

    public void LogError(LogEvent logEvent, Exception exception, string methodName, OrchestrationMessageReply orchestrationMessageReply)
    {
        if (Logger.IsEnabled(LogLevel.Error))
        {
            var orchestrationMessageReplyLogState = new OrchestrationMessageReplyLogState(logEvent, methodName, orchestrationMessageReply);
            Logger.Log(LogLevel.Error, new EventId((int)logEvent, logEvent.ToString()), orchestrationMessageReplyLogState, exception, OrchestrationMessageReplyLogState.Format);
        }
    }

    public void LogError(LogEvent logEvent, Exception exception, string methodName, BrokerMessage brokerMessage)
    {
        if (Logger.IsEnabled(LogLevel.Error))
        {
            var brokerMessageLogState = new BrokerMessageLogState(logEvent, methodName, brokerMessage);
            Logger.Log(LogLevel.Error, new EventId((int)logEvent, logEvent.ToString()), brokerMessageLogState, exception, BrokerMessageLogState.Format);
        }
    }
}
