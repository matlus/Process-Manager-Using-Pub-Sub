using Microsoft.AspNetCore.Mvc;
using PostBindOrchestrator.DomainLayer;

namespace PostBindOrchestrator.Api;

public sealed class PostBindRouteHandler
{
    private readonly DomainFacade domainFacade;

    public PostBindRouteHandler(DomainFacade domainFacade) => this.domainFacade = domainFacade;

    public async Task ProcessPostBind(string policyNumber, [FromHeader(Name = "X-Correlation-Id")] string correlationId, CorrelationIdProvider correlationIdProvider)
    {        
        await domainFacade.ProcessPostBind(correlationId, policyNumber, "");
    }
}
