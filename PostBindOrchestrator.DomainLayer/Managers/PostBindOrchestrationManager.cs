﻿using System;
using System.Text;
using Microsoft.Extensions.Logging;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;

internal sealed class PostBindOrchestrationManager : IDisposable
{
    private const string OrchestrationReplyTopicName = "pbo.orch.reply.topic";
    private const string OrchestrationReplyQueueName = "pbo.orch.reply.que";

    private readonly ServiceLocatorBase serviceLocator;
    private bool disposed;
    private ConfigurationProvider? configurationProvider;
    private ApplicationLogger? applicationLogger;
    private SubscriberBase? subscriberOrchestrationReply;

    private ConfigurationProvider ConfigurationProvider
    {
        get
        {
            return configurationProvider ??= serviceLocator.CreateConfigurationProvider();
        }
    }

    private ApplicationLogger ApplicationLogger
    {
        get
        {
            return applicationLogger ??= new ApplicationLogger(serviceLocator.CreateLogger());
        }
    }

    public PostBindOrchestrationManager(ServiceLocatorBase serviceLocator) => this.serviceLocator = serviceLocator;

    public Task ProcessPostBind(string correlationId, string policyNumber, string interviewData)
    {
        //// Validate formal arguments
        //// InterviewData needs to be a record type specified in this project. Use of string type is for demonstration purposes
        //// Should probably persist interview data in the database so Task Processors can access and simplifies "state" maintenace across threads (trigger and reply)
        return Task.CompletedTask;
    }

    public Task ProcessRevertToQuote(string correlationId, string policyNumber)
    {
        //// Validate formal arguments
        return Task.CompletedTask;
    }

    public async Task StartListening(CancellationToken cancellationToken)
    {
        var messageBrokerSettings = ConfigurationProvider.GetMessageBrokerSettings();

        subscriberOrchestrationReply = serviceLocator.CreateMessageBrokerSubscriber();
        await subscriberOrchestrationReply.Initialize(messageBrokerSettings.MessageBrokerConnectionString, OrchestrationReplyTopicName, OrchestrationReplyQueueName);
        await subscriberOrchestrationReply.Subscribe(OnOrchestrationReplyMessageReceived, cancellationToken);

        //// Start listening on the Exception Topic as well
    }

    private async Task OnOrchestrationReplyMessageReceived(SubscriberBase subscriber, MessageReceivedEventArgs messageReceivedEventArgs)
    {
        var brokerMessage = messageReceivedEventArgs.Message;
        OrchestrationMessageReply? orchestrationMessageReply = null;
        var logEvent = LogEvent.OnOrchestrationReplyMessageReceived;

        try
        {
            logEvent = LogEvent.DeSerializeReplyMessage;
            orchestrationMessageReply = ApplicationSerializer.Deserialize<OrchestrationMessageReply>(brokerMessage.Body);

            var orchestrationTaskNext = GetNextOrchestrationTask(orchestrationMessageReply.PolicyNumber, orchestrationMessageReply.OrchestrationTask);

            logEvent = LogEvent.OnOrchestrationTaskStartPublish;
            //// Publish message to trigger next task.
            //// Orchestrate the next Task (Publish OrchestrationTaskMessage to appropriate topic)
            //// Use OrchestrationTaskTopicDictionary to map task to topic

            await subscriber.Acknowledge(messageReceivedEventArgs.AcknowledgeToken, messageReceivedEventArgs.CancellationToken);
        }
        catch (MessageDeserializationFailedException e)
        {
            ApplicationLogger.LogError(logEvent, e, nameof(OnOrchestrationReplyMessageReceived), brokerMessage);
        }
        catch (Exception e)
        {
            ApplicationLogger.LogError(logEvent, e, nameof(OnOrchestrationReplyMessageReceived), orchestrationMessageReply!);
        }
    }

    private static OrchestrationTask GetNextOrchestrationTask(string policyNumber, OrchestrationTask orchestrationTask)
    {
        // Of course some logic needs to be here
        return NextOrchestrationTaskEnumProvider.GetNext(orchestrationTask);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing && !disposed)
        {
            subscriberOrchestrationReply?.Dispose();
            disposed = true;
        }
    }
}
