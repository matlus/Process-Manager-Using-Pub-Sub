namespace PostBindOrchestrator.DomainLayer.Managers.SegregatedInterfaces;

internal interface IHttpMessageHandlerProvider
{
    HttpMessageHandler CreateHttpMessageHandler();
}