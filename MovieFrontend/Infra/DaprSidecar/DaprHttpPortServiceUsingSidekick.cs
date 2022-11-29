using Man.Dapr.Sidekick;
using Man.Dapr.Sidekick.Process;

namespace MovieFrontend.Infra.DaprSidecar
{
    // registered as a transient service so fully reusable
    public class DaprHttpPortServiceUsingSidekick : IDaprHttpPortService
    {
        public DaprHttpPortServiceUsingSidekick(IDaprSidecarHost daprSidecarHost)
        {
            DaprHttpPort = daprSidecarHost.GetProcessOptions().DaprHttpPort;
        }

        public int? DaprHttpPort { get; }
    }
}