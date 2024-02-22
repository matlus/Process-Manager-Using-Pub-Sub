using System.Collections;

namespace PostBindOrchestrator.Core;

public sealed partial class ApplicationLogger
{
    private readonly record struct LogStateOrchestrationReplyMessage : IReadOnlyList<KeyValuePair<string, object>>
    {
        private readonly List<KeyValuePair<string, object>> keyValuePairs;

        public LogStateOrchestrationReplyMessage(
            LogEvent logEvent,
            string methodName,
            OrchestrationReplyMessage orchestrationReplyMessage,
            Exception? exception = default)
        {
            MethodName = methodName;
            OrchestrationReplyMessage = orchestrationReplyMessage;

            keyValuePairs = new List<KeyValuePair<string, object>>
                {
                    { new KeyValuePair<string, object>("EventId", (int)logEvent) },
                    { new KeyValuePair<string, object>("EventName", logEvent.ToString()) },
                    { new KeyValuePair<string, object>("MethodName", methodName) },
                    { new KeyValuePair<string, object>("CorrelationId", orchestrationReplyMessage.CorrelationId) },
                    { new KeyValuePair<string, object>("PolicyNumber", orchestrationReplyMessage.PolicyNumber) },
                    { new KeyValuePair<string, object>("OrchestrationTask", orchestrationReplyMessage.OrchestrationTask.ToString()) },
                    { new KeyValuePair<string, object>("TaskStage", orchestrationReplyMessage.TaskStage) },
                    { new KeyValuePair<string, object>("CreatedOn", orchestrationReplyMessage.CreatedOn.ToString("o")) },
                };

            if (exception is not null)
            {
                ExceptionType = exception.GetType().Name;
                keyValuePairs.Add(new KeyValuePair<string, object>("ExceptionType", ExceptionType));

                ExceptionCategory = exception is PostBindOrchestratorBaseException baseException ? baseException.Category : "Technical";
                keyValuePairs.Add(new KeyValuePair<string, object>("ExceptionCategory", ExceptionCategory));
            }
        }

        private string MethodName { get; }
        private string? ExceptionCategory { get; } = null;
        private string? ExceptionType { get; } = null;
        private OrchestrationReplyMessage OrchestrationReplyMessage { get; }

        public int Count => keyValuePairs.Count;

        public KeyValuePair<string, object> this[int index] => keyValuePairs[index];

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            var message =
                $" CorrelationId: `{OrchestrationReplyMessage.CorrelationId}`," +
                $" Policy Number: `{OrchestrationReplyMessage.PolicyNumber}`," +
                $" Orchestration Task: `{OrchestrationReplyMessage.OrchestrationTask}`," +
                $" Task Stage: `{OrchestrationReplyMessage.TaskStage}`," +
                $" Create On: `{OrchestrationReplyMessage.CreatedOn:o}`.";

            if (ExceptionType is not null)
            {
                message += $" Exception Type: {ExceptionType}, ";
            }

            if (ExceptionCategory is not null)
            {
                message += $" Exception Category: {ExceptionCategory}, ";
            }

            return message;
        }

        public static string Format(LogStateOrchestrationReplyMessage orchestrationMessageReplyLogState, Exception? exception)
        {
            return exception is not null
                ? $"An Exception of Type: `{orchestrationMessageReplyLogState.ExceptionType}` and Category: `{orchestrationMessageReplyLogState.ExceptionCategory}`, occurened in Method: `{orchestrationMessageReplyLogState.MethodName}`, with Message: {exception.Message}. Additional Information - " + orchestrationMessageReplyLogState.ToString()
                : orchestrationMessageReplyLogState.ToString();
        }
    }
}