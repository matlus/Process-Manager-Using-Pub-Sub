using PostBindOrchestrator.DomainLayer;

namespace PostBindOrchestrator.Api;

internal class RouteHandlerPostBind : RouteHandlerBase
{
    private readonly DomainFacade domainFacade;

    public RouteHandlerPostBind(DomainFacade domainFacade) => this.domainFacade = domainFacade;

    protected sealed override void MapRoutesCore(IEndpointRouteBuilder endpointRouteBuilder)
    {
        MapPostEndpoint(endpointRouteBuilder, "/processpostbind/{policyNumber}", "ProcessPostBind", ProcessPostBind);
    }

    public async Task ProcessPostBind(CorrelationIdProvider correlationIdProvider, string policyNumber, InterviewData interviewData)
    {
        await ProcessPostBindCore(correlationIdProvider.CorrelationId, policyNumber, interviewData);
    }

    protected virtual Task ProcessPostBindCore(string correlationId, string policyNumber, InterviewData interviewData)
    {
        return domainFacade.ProcessPostBind(correlationId, policyNumber, interviewData);
    }
}
