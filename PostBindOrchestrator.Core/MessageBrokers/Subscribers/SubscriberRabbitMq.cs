﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PostBindOrchestrator.Core;

public sealed class SubscriberRabbitMq : SubscriberBase
{
    private bool disposed;
    private IConnection connection = default!;
    private IModel channel = default!;
    private string queueName = default!;

    protected override Task InitializeCore(string connectionString, string topicName, string queueName, CancellationToken cancellationToken)
    {
        var connectionFactory = new ConnectionFactory()
        {
            Uri = new Uri(connectionString),
        };

        connection = connectionFactory.CreateConnection();
        channel = connection.CreateModel();

        channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind(queueName, topicName, routingKey: string.Empty, arguments: null);

        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        this.queueName = queueName;

        return Task.CompletedTask;
    }

    protected override Task SubscribeCore(Func<SubscriberBase, MessageReceivedEventArgs, Task> receiveCallback, CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            var messageType = ea.BasicProperties.Type;
            var brokerMessage = new BrokerMessage(messageType, ea.BasicProperties.CorrelationId, ea.Body.ToArray());

            var messageReceivedEventArgs = new MessageReceivedEventArgs(brokerMessage, ea.DeliveryTag, cancellationToken);

            await receiveCallback(this, messageReceivedEventArgs);
        };

        channel.BasicConsume(queueName, autoAck: false, consumer);

        return Task.CompletedTask;
    }

    protected override Task AcknowledgeCore(object acknowledgetoken, CancellationToken cancellationToken)
    {
        channel.BasicAck((ulong)acknowledgetoken, multiple: false);

        return Task.CompletedTask;
    }

    protected override Task Dispose(bool disposing)
    {
        if (disposing && !disposed)
        {
            channel.Close();
            channel.Dispose();
            
            connection.Close();
            connection.Dispose();

            disposed = true;
        }

        return Task.CompletedTask;
    }
}