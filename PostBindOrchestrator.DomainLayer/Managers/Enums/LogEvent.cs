namespace PostBindOrchestrator.DomainLayer;
public enum LogEvent
{
    OnStartListening,    
    OnOrchestrationReplyMessageReceived,
    DeSerializeReplyMessage,
    OnOrchestrationTaskStartPublish,
}
