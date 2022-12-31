using System.Threading.Tasks;
using PostBindOrchestrator.Api;
using PostBindOrchestrator.DomainLayer;

namespace PostBindOrchestrator.MiddlewareTests;

internal sealed class RouteHandlerPostBindSpy : RouteHandlerPostBind, ITestMediatorCompositor
{
    public TestMediator TestMediator { get; }

    public RouteHandlerPostBindSpy(DomainFacade domainFacade) : base(domainFacade) => TestMediator = new TestMediator();

    protected override Task ProcessPostBindCore(string correlationId, string policyNumber, InterviewData interviewData)
    {
        TestMediator.CorrelationId = correlationId;
        TestMediator.PolicyNumber = policyNumber;
        TestMediator.InterviewData = interviewData;
        return Task.CompletedTask;
    }
}
