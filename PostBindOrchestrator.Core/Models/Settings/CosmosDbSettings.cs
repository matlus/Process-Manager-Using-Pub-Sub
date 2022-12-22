namespace PostBindOrchestrator.Core;

public sealed record CosmosDbSettings(string DatabaseName, string CosmosDbConnectionString);