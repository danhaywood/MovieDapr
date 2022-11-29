namespace MovieFrontend.Infra.DaprSidecar;

// registered as a transient service so fully reusable
public class DaprHttpPortServiceUsingEnvVar : IDaprHttpPortService
{
    public DaprHttpPortServiceUsingEnvVar()
    {
        DaprHttpPort = int.Parse(Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3500");
    }

    public int? DaprHttpPort { get; }
}