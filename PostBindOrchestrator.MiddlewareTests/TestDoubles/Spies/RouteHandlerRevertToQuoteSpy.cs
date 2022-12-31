using System.Threading.Tasks;
using PostBindOrchestrator.Api;
using PostBindOrchestrator.DomainLayer;

namespace PostBindOrchestrator.MiddlewareTests;
internal sealed class RouteHandlerRevertToQuoteSpy : RouteHandlerRevertToQuote, ITestMediatorCompositor
{
    public TestMediator TestMediator { get; }

    public RouteHandlerRevertToQuoteSpy(DomainFacade domainFacade) : base(domainFacade) => TestMediator = new TestMediator();

    protected override Task ProcessRevertToQuoteCore(string correlationId, string policyNumber)
    {
        TestMediator.CorrelationId = correlationId;
        TestMediator.PolicyNumber = policyNumber;
        return Task.CompletedTask;
    }
}
