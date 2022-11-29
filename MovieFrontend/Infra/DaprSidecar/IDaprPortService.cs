using Man.Dapr.Sidekick;

namespace MovieFrontend.Infra.DaprSidecar;

// registered as a transient service so fully reusable
public interface IDaprHttpPortService
{
    public int? DaprHttpPort { get; }    
}