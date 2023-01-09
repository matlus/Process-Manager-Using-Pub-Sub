namespace PostBindOrchestrator.Core;

public sealed record OrchestrationReplyMessage(string Id, string CorrelationId, string PolicyNumber, DateTimeOffset CreatedOn, OrchestrationTask OrchestrationTask, string TaskStage);