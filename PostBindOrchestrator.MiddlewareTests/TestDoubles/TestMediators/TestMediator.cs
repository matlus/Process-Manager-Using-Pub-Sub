using System;
using Microsoft.Extensions.Logging;
using PostBindOrchestrator.DomainLayer;

namespace PostBindOrchestrator.MiddlewareTests;
public sealed class TestMediator
{
    public string? CorrelationId { get; set; }
    public string? PolicyNumber { get; set; }
    public InterviewData? InterviewData { get; set; }
}
