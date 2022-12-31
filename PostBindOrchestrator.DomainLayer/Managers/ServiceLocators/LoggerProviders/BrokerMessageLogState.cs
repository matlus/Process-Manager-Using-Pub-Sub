using System.Collections;
using System.Text;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;

public sealed partial class ApplicationLogger
{
    public readonly record struct BrokerMessageLogState : IReadOnlyList<KeyValuePair<string, object>>
    {
        private readonly List<KeyValuePair<string, object>> keyValuePairs;

        public BrokerMessageLogState(
            LogEvent logEvent,
            string methodName,
            BrokerMessage brokerMessage)
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
        }

        private string MethodName { get; }
        private BrokerMessage BrokerMessage { get; }
        private string MessageBody { get; }

        public int Count => keyValuePairs.Count;

        public KeyValuePair<string, object> this[int index] => index switch
        {
            0 => keyValuePairs[0],
            1 => keyValuePairs[1],
            2 => keyValuePairs[2],
            3 => keyValuePairs[3],
            4 => keyValuePairs[4],
            5 => keyValuePairs[5],
            _ => throw new IndexOutOfRangeException(nameof(index)),
        };

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            var errorLogString = new StringBuilder();

            errorLogString.AppendLine($"Error Log from Method: `{MethodName}`, CorrelationId: `{BrokerMessage.CorrelationId}`," +
                $"MessageType: `{BrokerMessage.MessageType}`, Body: `{MessageBody}`");

            return errorLogString.ToString();
        }

        public static string Format(BrokerMessageLogState brokerMessageLogState, Exception? exception)
        {
            return (exception is not null)
                ? brokerMessageLogState.ToString() + $" Original Exception Type: {exception.GetType().Name}, with message: {exception.Message}"
                : brokerMessageLogState.ToString();
        }
    }
}
