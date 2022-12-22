namespace PostBindOrchestrator.Core;

public sealed record BrokerMessage(string MessageType, byte[] Body);