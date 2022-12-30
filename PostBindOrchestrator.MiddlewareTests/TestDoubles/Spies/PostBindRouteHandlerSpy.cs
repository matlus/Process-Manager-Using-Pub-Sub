using System.Threading.Tasks;
using PostBindOrchestrator.Api;
using PostBindOrchestrator.DomainLayer;

namespace PostBindOrchestrator.MiddlewareTests;

internal sealed class PostBindRouteHandlerSpy : PostBindRouteHandler, ITestMediatorCompositor
{
    private readonly TestMediator testMediator;
    public TestMediator TestMediator => testMediator;

    public PostBindRouteHandlerSpy(DomainFacade domainFacade) : base(domainFacade) => testMediator = new TestMediator();

    protected override Task ProcessPostBindCore(string correlationId, string policyNumber, InterviewData interviewData)
    {
        testMediator.CorrelationId = correlationId;
        testMediator.PolicyNumber = policyNumber;
        testMediator.InterviewData = interviewData;
        return Task.CompletedTask;
    }
}
