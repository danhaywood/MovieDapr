using Dapr.Client;

namespace MovieBackend.Services;

// registered as a transient service so fully reusable
public class ConnectionStringService
{
    public ConnectionStringService()
    {
        var daprClient = new DaprClientBuilder().Build();
        
        // might be worth wrapping with Polly for retry ?
        var secretAsync = daprClient.GetSecretAsync("movie-secret-store", "ConnectionString");
        var connectionString = secretAsync.Result["ConnectionString"];
        
        ConnectionString = connectionString;
    }

    public string ConnectionString { get; }
}