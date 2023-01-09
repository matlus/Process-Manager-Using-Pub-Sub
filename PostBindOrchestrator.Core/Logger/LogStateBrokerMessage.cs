using System.Collections;
using System.Text;

namespace PostBindOrchestrator.Core;

public sealed partial class ApplicationLogger
{
    private readonly record struct LogStateBrokerMessage : IReadOnlyList<KeyValuePair<string, object>>
    {
        private readonly List<KeyValuePair<string, object>> keyValuePairs;

        public LogStateBrokerMessage(
            LogEvent logEvent,
            string methodName,
            BrokerMessage brokerMessage,
            Exception? exception = default)
        {
            MethodName = methodName;
            BrokerMessage = brokerMessage;
            MessageBody = Encoding.UTF8.GetString(brokerMessage.Body);

            keyValuePairs = new List<KeyValuePair<string, object>>
                {
                    { new KeyValuePair<string, object>("EventId", (int)logEvent) },
                    { new KeyValuePair<string, object>("EventName", logEvent.ToString()) },
                    { new KeyValuePair<string, object>("MethodName", methodName) },
                    { new KeyValuePair<string, object>("CorrelationId", brokerMessage.CorrelationId) },
                    { new KeyValuePair<string, object>("MessageType", brokerMessage.MessageType) },
                    { new KeyValuePair<string, object>("Body", MessageBody) },
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
        private BrokerMessage BrokerMessage { get; }
        private string MessageBody { get; }

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
                $" CorrelationId: `{BrokerMessage.CorrelationId}`," +
                $" MessageType: `{BrokerMessage.MessageType}`," +
                $" Body: `{MessageBody}`,";

            if (ExceptionType is not null)
            {
                message += $" Exception Type: {ExceptionType} , ";
            }

            if (ExceptionCategory is not null)
            {
                message += $" Exception Category: {ExceptionCategory} , ";
            }

            return message;
        }

        public static string Format(LogStateBrokerMessage brokerMessageLogState, Exception? exception)
        {
            return exception is not null
                ? $"An Exception of Type: `{brokerMessageLogState.ExceptionType}` and Category: `{brokerMessageLogState.ExceptionCategory}`, occurened in Method: `{brokerMessageLogState.MethodName}`, with Message: {exception.Message}. Additional Information - " + brokerMessageLogState.ToString()
                : brokerMessageLogState.ToString();
        }
    }
}
