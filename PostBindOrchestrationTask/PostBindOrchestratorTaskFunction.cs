using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PostBindOrchestrationTask.DomainLayer;

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
        [ServiceBusTrigger("pbo.orch.task1.topic", "pbo.orch.task1.queue.azurefuncprocessor", Connection = "ServiceBusConnectionString")] byte[] message,
        Dictionary<string, object> applicationProperties,
        FunctionContext functionContext,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Entered function: {Function Name}", nameof(PostBindOrchestratorTask_Abc));
        var serviceBusReceivedMessage = MapToServiceBusReceivedMessage(message, applicationProperties, functionContext.BindingContext.BindingData);

        await domainFacade.StartTask(serviceBusReceivedMessage, cancellationToken);
    }

    private static ServiceBusReceivedMessage MapToServiceBusReceivedMessage(byte[] message, Dictionary<string, object> applicationProperties, IReadOnlyDictionary<string, object?> bindingData)
    {
        var body = BinaryData.FromBytes(message);
        var messageId = (string)bindingData["MessageId"]!;
        var contentType = (string)bindingData["ContentType"]!;
        var correlationId = (string)bindingData["CorrelationId"]!;
        var deliveryCount = int.Parse((string)bindingData["DeliveryCount"]!);
        var lockToken = Guid.Parse((string)bindingData["LockToken"]!);
        return ServiceBusModelFactory.ServiceBusReceivedMessage(body, messageId, correlationId: correlationId, properties: applicationProperties, lockTokenGuid: lockToken, deliveryCount: deliveryCount, contentType: contentType);
    }
}