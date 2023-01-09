using System.Collections;

namespace PostBindOrchestrator.Core;

public sealed partial class ApplicationLogger
{
    private readonly record struct LogStateOrchestrationTaskMessage : IReadOnlyList<KeyValuePair<string, object>>
    {
        private readonly List<KeyValuePair<string, object>> keyValuePairs;

        public LogStateOrchestrationTaskMessage(
            LogEvent logEvent,
            string methodName,
            OrchestrationTaskMessage orchestrationTaskMessage,
            Exception? exception = default)
        {
            MethodName = methodName;
            OrchestrationTaskMessage = orchestrationTaskMessage;

            keyValuePairs = new List<KeyValuePair<string, object>>
                {
                    { new KeyValuePair<string, object>("EventId", (int)logEvent) },
                    { new KeyValuePair<string, object>("EventName", logEvent.ToString()) },
                    { new KeyValuePair<string, object>("MethodName", methodName) },
                    { new KeyValuePair<string, object>("CorrelationId", orchestrationTaskMessage.CorrelationId) },
                    { new KeyValuePair<string, object>("PolicyNumber", orchestrationTaskMessage.PolicyNumber) },
                    { new KeyValuePair<string, object>("OrchestrationTask", orchestrationTaskMessage.OrchestrationTask.ToString()) },
                    { new KeyValuePair<string, object>("CreatedOn", orchestrationTaskMessage.CreatedOn.ToString("o")) },
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
        private OrchestrationTaskMessage OrchestrationTaskMessage { get; }

        public int Count => keyValuePairs.Count;

        public KeyValuePair<string, object> this[int index] => index < 0 || index >= Count ? throw new IndexOutOfRangeException(nameof(index)) : keyValuePairs[index];

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
                $" CorrelationId: `{OrchestrationTaskMessage.CorrelationId}`," +
                $" Policy Number: `{OrchestrationTaskMessage.PolicyNumber}`," +
                $" Orchestration Task: `{OrchestrationTaskMessage.OrchestrationTask}`," +
                $" Create On: `{OrchestrationTaskMessage.CreatedOn:o}`.";

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

        public static string Format(LogStateOrchestrationTaskMessage orchestrationMessageReplyLogState, Exception? exception)
        {
            return exception is not null
                ? $"An Exception of Type: `{orchestrationMessageReplyLogState.ExceptionType}` and Category: `{orchestrationMessageReplyLogState.ExceptionCategory}`, occurened in Method: `{orchestrationMessageReplyLogState.MethodName}`, with Message: {exception.Message}. Additional Information - " + orchestrationMessageReplyLogState.ToString()
                : orchestrationMessageReplyLogState.ToString();
        }
    }
}