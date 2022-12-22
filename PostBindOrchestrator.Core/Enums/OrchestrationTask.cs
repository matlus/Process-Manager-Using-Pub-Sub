namespace PostBindOrchestrator.Core;

public enum OrchestrationTask
{
    None,
    RevertToQuote,
    PayplanEnrollment,
    CreateClient,
    AssociateClientWithPolicy,
    UpdateIes,
    TelematicsEnrollment,
    AutoRpmEnrollment
}
