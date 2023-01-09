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
    OrchestrationTaskStartRecorded,
    OrchestrationTaskFinished,
    OrchestrationTaskFinishedRecorded,
    OrchestrationTaskStartPublished,
    OrchestrationTaskReplyPublished
}
