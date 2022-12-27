namespace PostBindOrchestrator.Core;

public sealed record BrokerMessage(string MessageType, string CorrelationId, byte[] Body);