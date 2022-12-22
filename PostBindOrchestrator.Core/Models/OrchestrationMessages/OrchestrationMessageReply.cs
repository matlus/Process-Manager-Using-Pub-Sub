namespace PostBindOrchestrator.Core;

public sealed record OrchestrationMessageReply(string Id, string CorrelationId, string PolicyNumber, DateTimeOffset CreatedOn, OrchestrationTask OrchestrationTask, string TaskStage);