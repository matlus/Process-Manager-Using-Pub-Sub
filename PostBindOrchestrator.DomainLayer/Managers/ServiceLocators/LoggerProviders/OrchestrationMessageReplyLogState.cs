using System.Collections;
using System.Text;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;

public sealed partial class ApplicationLogger
{
    private readonly record struct OrchestrationMessageReplyLogState : IReadOnlyList<KeyValuePair<string, object>>
    {
        private readonly List<KeyValuePair<string, object>> keyValuePairs;

        public OrchestrationMessageReplyLogState(
            LogEvent logEvent,
            string methodName,
            OrchestrationMessageReply orchestrationMessageReply)
        {
            MethodName = methodName;
            OrchestrationMessageReply = orchestrationMessageReply;

            keyValuePairs = new List<KeyValuePair<string, object>>
                {
                    { new KeyValuePair<string, object>("EventId", (int)logEvent) },
                    { new KeyValuePair<string, object>("EventName", logEvent.ToString()) },
                    { new KeyValuePair<string, object>("MethodName", methodName) },
                    { new KeyValuePair<string, object>("CorrelationId", orchestrationMessageReply.CorrelationId) },
                    { new KeyValuePair<string, object>("PolicyNumber", orchestrationMessageReply.PolicyNumber) },
                    { new KeyValuePair<string, object>("OrchestrationTask", orchestrationMessageReply.OrchestrationTask.ToString()) },
                    { new KeyValuePair<string, object>("CreatedOn", orchestrationMessageReply.CreatedOn.ToString("o")) },
                };
        }

        private string MethodName { get; }
        private OrchestrationMessageReply OrchestrationMessageReply { get; }

        public int Count => keyValuePairs.Count;

        public KeyValuePair<string, object> this[int index] => index switch
        {
            0 => keyValuePairs[0],
            1 => keyValuePairs[1],
            2 => keyValuePairs[2],
            3 => keyValuePairs[3],
            4 => keyValuePairs[4],
            5 => keyValuePairs[5],
            6 => keyValuePairs[6],
            _ => throw new IndexOutOfRangeException(nameof(index)),
        };

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
            var errorLogString = new StringBuilder();

            errorLogString.AppendLine($"Error Log from Method: `{MethodName}`, CorrelationId: `{OrchestrationMessageReply.CorrelationId}`," +
                $"Policy Number: `{OrchestrationMessageReply.PolicyNumber}`, Orchestration Task: `{OrchestrationMessageReply.OrchestrationTask}`, " +
                $"Create On: `{OrchestrationMessageReply.TaskStage}`");

            return errorLogString.ToString();
        }

        public static string Format(OrchestrationMessageReplyLogState orchestrationMessageReplyLogState, Exception? exception)
        {
            return (exception is not null)
                ? orchestrationMessageReplyLogState.ToString() + $" Original Exception Type: {exception.GetType().Name}, with message: {exception.Message}"
                : orchestrationMessageReplyLogState.ToString();
        }
    }
}