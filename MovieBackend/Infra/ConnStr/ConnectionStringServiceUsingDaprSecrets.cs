using Dapr.Client;

namespace MovieBackend.Infra.ConnStr;

// registered as a transient service so fully reusable
public class ConnectionStringServiceUsingDaprSecrets : IConnectionStringService
{
    public ConnectionStringServiceUsingDaprSecrets()
    {
        var daprClient = new DaprClientBuilder().Build();
        
        // might be worth wrapping with Polly for retry ?
        var secretAsync = daprClient.GetSecretAsync("movie-secret-store", "ConnectionString");
        var connectionString = secretAsync.Result["ConnectionString"];
        
        ConnectionString = connectionString;
    }

    public string ConnectionString { get; }
}