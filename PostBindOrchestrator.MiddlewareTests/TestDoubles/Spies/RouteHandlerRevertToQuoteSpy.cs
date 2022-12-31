using System.Threading.Tasks;
using PostBindOrchestrator.Api;
using PostBindOrchestrator.DomainLayer;

namespace PostBindOrchestrator.MiddlewareTests;
internal sealed class RouteHandlerRevertToQuoteSpy : RouteHandlerRevertToQuote, ITestMediatorCompositor
{
    private readonly TestMediator testMediator;
    public TestMediator TestMediator => testMediator;

    public RouteHandlerRevertToQuoteSpy(DomainFacade domainFacade) : base(domainFacade) => testMediator = new TestMediator();

    protected override Task ProcessRevertToQuoteCore(string correlationId, string policyNumber)
    {
        testMediator.CorrelationId = correlationId;
        testMediator.PolicyNumber = policyNumber;
        return Task.CompletedTask;
    }
}
