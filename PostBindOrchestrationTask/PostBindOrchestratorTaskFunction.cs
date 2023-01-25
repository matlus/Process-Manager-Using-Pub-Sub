using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PostBindOrchestrationTask.DomainLayer;
using PostBindOrchestrator.Core;

namespace PostBindOrchestrationTask;

public class PostBindOrchestratorTaskFunction
{
    private readonly ILogger _logger;
    private readonly DomainFacade domainFacade;

    public PostBindOrchestratorTaskFunction(DomainFacade domainFacade, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<PostBindOrchestratorTaskFunction>();
        this.domainFacade = domainFacade;
    }

    [Function(nameof(PostBindOrchestratorTask_Abc))]
    public async Task PostBindOrchestratorTask_Abc(
        [ServiceBusTrigger("pbo.orch.sendcoidocument.topic", "pbo.orch.sendcoidocument.queue.azurefuncprocessor", Connection = "ServiceBusConnectionString")] byte[] message,
        Dictionary<string, object> applicationProperties,
        FunctionContext functionContext,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Entered function: {Function Name}", nameof(PostBindOrchestratorTask_Abc));
        await domainFacade.StartTask(message, cancellationToken);

        //// The following are two other options *if needed*. Essentially, showing how one could get
        //// the entire ServiceBusReceivedMessage or the custom BrokerMessage class that is referenced in other parts of the system
        //// If either of these two options is not used, then also remove the extra method arguments from the function method

        //// Broker Message:
        ////var brokerMessage = MapToBrokerMessage(message, applicationProperties, functionContext.BindingContext.BindingData);
        ////await domainFacade.StartTask(brokerMessage, cancellationToken);        

        //// ServiceBusReceivedMessage:
        ////var serviceBusReceivedMessage = MapToServiceBusReceivedMessage(message, applicationProperties, functionContext.BindingContext.BindingData);
        ////await domainFacade.StartTask(serviceBusReceivedMessage, cancellationToken);
    }

    /*
    ******************************
    ***** See Comments above *****
    ******************************
    */
    ////private static BrokerMessage MapToBrokerMessage(byte[] message, Dictionary<string, object> applicationProperties, IReadOnlyDictionary<string, object?> bindingData)
    ////{
    ////    var messageType = GetHeaderValue(applicationProperties, "MessageType");
    ////    var correlationId = (string)bindingData["CorrelationId"]!;
    ////    return new BrokerMessage(messageType, correlationId, message);
    ////}

    ////private static ServiceBusReceivedMessage MapToServiceBusReceivedMessage(byte[] message, Dictionary<string, object> applicationProperties, IReadOnlyDictionary<string, object?> bindingData)
    ////{
    ////    var body = BinaryData.FromBytes(message);
    ////    var messageId = (string)bindingData["MessageId"]!;
    ////    var contentType = (string)bindingData["ContentType"]!;
    ////    var correlationId = (string)bindingData["CorrelationId"]!;
    ////    var deliveryCount = int.Parse((string)bindingData["DeliveryCount"]!);
    ////    var lockToken = Guid.Parse((string)bindingData["LockToken"]!);
    ////    return ServiceBusModelFactory.ServiceBusReceivedMessage(body, messageId, correlationId: correlationId, properties: applicationProperties, lockTokenGuid: lockToken, deliveryCount: deliveryCount, contentType: contentType);
    ////}

    ////private static string GetHeaderValue(IReadOnlyDictionary<string, object> dictionary, string key)
    ////{
    ////    return dictionary.TryGetValue(key, out var value)
    ////        ? (string)value
    ////        : throw new MessageHeaderKeyNotFoundException($"The Message Header with Key: {key}, was Not Found in the Collection of Headers");
    ////}
}