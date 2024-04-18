namespace PostBindOrchestrator.Core;

public sealed record MessageBrokerSettings(string ConnectionString, MessageBrokerType MessageBrokerType);