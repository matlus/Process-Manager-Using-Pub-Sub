using Microsoft.Extensions.Logging;
using PostBindOrchestrator.Core;
using System.Diagnostics.CodeAnalysis;

namespace PostBindOrchestrator.DomainLayer;

[ExcludeFromCodeCoverage]
public sealed partial class ApplicationLogger
{
    private readonly ILogger logger;
    public ILogger Logger => logger;

    public ApplicationLogger(ILogger logger) => this.logger = logger;

    public void LogInformation(int eventId, string? message, params object?[] args)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
#pragma warning disable CA2254 // Template should be a static expression
            logger.LogInformation(eventId, message, args);
#pragma warning restore CA2254 // Template should be a static expression
        }
    }

    public void LogError<T1, T2, T3>(LogEvent logEvent, Exception exception, string? message, T1 param1, T2 param2, T3 param3)
    {
        if (logger.IsEnabled(LogLevel.Error))
        {
#pragma warning disable CA2254 // Template should be a static expression
            logger.LogError((int)logEvent, exception, message, param1, param2, param3);
#pragma warning restore CA2254 // Template should be a static expression
        }
    }

    public void LogError<T1, T2, T3, T4, T5>(LogEvent logEvent, Exception exception, string? message, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
    {
        if (logger.IsEnabled(LogLevel.Error))
        {
#pragma warning disable CA2254 // Template should be a static expression
            logger.LogError((int)logEvent, exception, message, param1, param2, param3, param4, param5);
#pragma warning restore CA2254 // Template should be a static expression
        }
    }

    public void LogError(LogEvent logEvent, Exception exception, string methodName, OrchestrationMessageReply orchestrationMessageReply)
    {
        if (logger.IsEnabled(LogLevel.Error))
        {
            var orchestrationMessageReplyLogState = new OrchestrationMessageReplyLogState(logEvent, methodName, orchestrationMessageReply);
            logger.Log(LogLevel.Error, new EventId((int)logEvent, logEvent.ToString()), orchestrationMessageReplyLogState, exception, OrchestrationMessageReplyLogState.Format);
        }
    }

    public void LogError(LogEvent logEvent, Exception exception, string methodName, BrokerMessage brokerMessage)
    {
        if (logger.IsEnabled(LogLevel.Error))
        {
            var brokerMessageLogState = new BrokerMessageLogState(logEvent, methodName, brokerMessage);
            logger.Log(LogLevel.Error, new EventId((int)logEvent, logEvent.ToString()), brokerMessageLogState, exception, BrokerMessageLogState.Format);
        }
    }
}
