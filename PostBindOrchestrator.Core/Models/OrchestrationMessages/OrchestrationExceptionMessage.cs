namespace PostBindOrchestrator.Core.Models.OrchestrationMessages;

public sealed record OrchestrationExceptionMessage(string Id, string CorrelationId, string PolicyNumber, DateTimeOffset CreatedOn, OrchestrationTask OrchestrationTask, LogEvent LogEvent, ExceptionData ExceptionData);
