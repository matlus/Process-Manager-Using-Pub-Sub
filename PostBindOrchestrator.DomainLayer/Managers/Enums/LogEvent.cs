namespace PostBindOrchestrator.DomainLayer;
public enum LogEvent
{
    Middleware,
    OnStartListening,
    OnOrchestrationReplyMessageReceived,
    DeSerializeReplyMessage,
    OnOrchestrationTaskStartPublish,
}
