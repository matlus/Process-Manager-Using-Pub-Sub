namespace PostBindOrchestrator.Core;

public enum OrchestrationTask
{
    None,
    RevertToQuote,
    PayplanEnrollment,
    CreateClient,
    AssociateClientWithPolicy,
    SendCoIDocument,
    UpdateIds,
    TelematicsEnrollment,
    AutoRpmEnrollment
}
