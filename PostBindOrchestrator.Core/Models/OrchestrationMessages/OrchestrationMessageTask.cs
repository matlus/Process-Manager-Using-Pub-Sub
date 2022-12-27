namespace PostBindOrchestrator.Core;

public sealed record OrchestrationMessageTask(string Id, string CorrelationId, string PolicyNumber, DateTimeOffset CreatedOn, OrchestrationTask OrchestrationTask);
