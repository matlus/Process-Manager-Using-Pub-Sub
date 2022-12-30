using PostBindOrchestrator.DomainLayer;

namespace PostBindOrchestrator.Api;

internal class RevertToQuoteRouteHandler : RouteHandlerBase
{
    private readonly DomainFacade domainFacade;

    public RevertToQuoteRouteHandler(DomainFacade domainFacade) => this.domainFacade = domainFacade;

    protected sealed override void MapRoutesCore(IEndpointRouteBuilder endpointRouteBuilder)
    {
        MapPostEndpoint(endpointRouteBuilder, "/processreverttoquote/{policyNumber}", "ProcessRevertToQuote", ProcessRevertToQuote);        
    }

    public async Task ProcessRevertToQuote(CorrelationIdProvider correlationIdProvider, string policyNumber)
    {
        await ProcessRevertToQuoteCore(correlationIdProvider.CorrelationId, policyNumber);
    }

    protected virtual Task ProcessRevertToQuoteCore(string correlationId, string policyNumber)
    {
        return domainFacade.ProcessRevertToQuote(correlationId, policyNumber);
    }
}

