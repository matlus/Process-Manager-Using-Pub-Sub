namespace PostBindOrchestrator.Core;
public enum LogEvent
{
    Middleware,
    OnStartListening,
    StartTask,
    DeSerializeMessage,
    OrchestrationTaskMessageReceived,
    OrchestrationReplyMessageReceived,
    OrchestrationTaskStarted,
    OrchestrationTaskStartRecord,
    OrchestrationTaskFinishedRecord,
    OrchestrationTaskStartPublish,
    OrchestrationTaskReplyPublish
}
