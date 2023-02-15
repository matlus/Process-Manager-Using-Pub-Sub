using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace PostBindOrchestrator.Api.Providers;

internal sealed class RoleNameTelemetryInitializer : ITelemetryInitializer
{
    private readonly string roleName;

    public RoleNameTelemetryInitializer(string roleName) => this.roleName = roleName;

    public void Initialize(ITelemetry telemetry)
    {
        telemetry.Context.Cloud.RoleName = roleName;
    }
}