using System;
using Microsoft.Extensions.Logging;

namespace PostBindOrchestrator.MiddlewareTests;

public sealed class LoggerSpy : ILogger
{
    public TestMediatorLogger TestMediatorLogger { get; }

    public LoggerSpy() => TestMediatorLogger = new TestMediatorLogger();

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        TestMediatorLogger.LogLevel = logLevel;
        TestMediatorLogger.EventId = eventId;
        TestMediatorLogger.State = state;
        TestMediatorLogger.Exception = exception;
        TestMediatorLogger.LogMessage = formatter(state, exception);
    }
}

public sealed class LoggerProviderSpy : ILoggerProvider
{
    public LoggerSpy LoggerSpy { get; }

    public LoggerProviderSpy() => LoggerSpy = new LoggerSpy();

    public ILogger CreateLogger(string categoryName) => LoggerSpy;

    public void Dispose()
    {
    }
}