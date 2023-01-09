using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PostBindOrchestrator.MiddlewareTests;

internal sealed class LoggerSpy : ILogger
{
    public TestMediator TestMediator { get; }

    public LoggerSpy(TestMediator testMediator) => TestMediator = testMediator;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => throw new NotImplementedException();
    public bool IsEnabled(LogLevel logLevel) => throw new NotImplementedException();
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {

    }    
}
