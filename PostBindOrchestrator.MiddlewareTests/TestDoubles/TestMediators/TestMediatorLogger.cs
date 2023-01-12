using System;
using Microsoft.Extensions.Logging;

namespace PostBindOrchestrator.MiddlewareTests;

public sealed class TestMediatorLogger
{
    public LogLevel? LogLevel { get; set; }
    public EventId? EventId { get; set; }
    public object? State { get; set; }
    public Exception? Exception { get; set; }
    public string? LogMessage { get; set; }
}
