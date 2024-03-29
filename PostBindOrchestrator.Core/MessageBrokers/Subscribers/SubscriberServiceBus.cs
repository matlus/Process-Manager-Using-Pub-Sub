﻿using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace PostBindOrchestrator.Core;

public sealed class SubscriberServiceBus : SubscriberBase
{
    private static readonly ServiceBusClientOptions serviceBusClientOptions = new()
    {
        TransportType = ServiceBusTransportType.AmqpWebSockets
    };

    private bool disposed;
    private ServiceBusClient serviceBusClient = default!;
    private ServiceBusReceiver serviceBusReceiver = default!;

    protected override async Task InitializeCore(string connectionString, string topicName, string queueName, CancellationToken cancellationToken)
    {
        TopicName = topicName;
        QueueName = queueName;

        await CreateQueueIfNotExists(connectionString, cancellationToken);

        serviceBusClient = new ServiceBusClient(connectionString, serviceBusClientOptions);
        serviceBusReceiver = serviceBusClient.CreateReceiver(TopicName, QueueName);
    }

    protected override async Task SubscribeCore(Func<SubscriberBase, MessageReceivedEventArgs, Task> receiveCallback, CancellationToken cancellationToken)
    {
        await foreach (var serviceBusReceivedMessage in serviceBusReceiver.ReceiveMessagesAsync(cancellationToken))
        {
            var messageType = GetHeaderValue(serviceBusReceivedMessage.ApplicationProperties, "MessageType");
            var bytes = serviceBusReceivedMessage.Body.ToArray();
            var brokerMessage = new BrokerMessage(messageType, serviceBusReceivedMessage.CorrelationId, bytes);

            var messageReceivedEventArgs = new MessageReceivedEventArgs(brokerMessage, serviceBusReceivedMessage, cancellationToken);

            await receiveCallback(this, messageReceivedEventArgs);
        }
    }

    protected override async Task AcknowledgeCore(object acknowledgetoken, CancellationToken cancellationToken)
    {
        await serviceBusReceiver.CompleteMessageAsync((ServiceBusReceivedMessage)acknowledgetoken, cancellationToken);
    }

    protected override async Task Dispose(bool disposing)
    {
        if (disposing && !disposed)
        {
            await serviceBusClient.DisposeAsync();
            disposed = true;
        }
    }

    private async Task CreateQueueIfNotExists(string connectionString, CancellationToken cancellationToken)
    {
        var serviceBusAdministrationClient = new ServiceBusAdministrationClient(connectionString);

        var response = await serviceBusAdministrationClient.SubscriptionExistsAsync(TopicName, QueueName, cancellationToken);
        var subscriptionExists = response.Value;

        if (!subscriptionExists)
        {
            await serviceBusAdministrationClient.CreateSubscriptionAsync(TopicName, QueueName, cancellationToken);
        }
    }

    private static string GetHeaderValue(IReadOnlyDictionary<string, object> dictionary, string key)
    {
        return dictionary.TryGetValue(key, out var value)
            ? (string)value
            : throw new MessageHeaderKeyNotFoundException($"The Message Header with Key: {key}, was Not Found in the Collection of Headers");
    }
}
