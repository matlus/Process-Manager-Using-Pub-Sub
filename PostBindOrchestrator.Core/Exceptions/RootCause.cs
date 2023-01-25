namespace PostBindOrchestrator;

public record RootCause(string WellKnownSource, string WellKnownTarget, string ExceptionType, string ExceptionCategory, string ExceptionMessage);
