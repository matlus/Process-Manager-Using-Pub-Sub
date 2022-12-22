namespace PostBindOrchestrator.Core;

public sealed record OrchestrationTaskMessage(string Id, string CorrelationId, string PolicyNumber, DateTimeOffset CreatedOn, OrchestrationTask OrchestrationTask);
