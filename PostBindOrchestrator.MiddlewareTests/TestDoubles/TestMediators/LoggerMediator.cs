using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PostBindOrchestrator.MiddlewareTests;

internal sealed class LoggerMediator<TState>
{
    public LogLevel? LogLevel { get; set; }
    public EventId? EventId { get; set; }
    public TState? State { get; set; }    
    public Exception? Exception { get; set; }
    Func<TState, Exception?, string>? Formatter {  get; set; }
}
