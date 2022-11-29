using Dapr.Client;
using Polly;

namespace MovieBackend.Services;

public class ConnectionStringService
{
    public ConnectionStringService()
    {
        var daprClient = new DaprClientBuilder().Build();
        var secretAsync = daprClient.GetSecretAsync("movie-secret-store", "ConnectionString");
        var connectionString = secretAsync.Result["ConnectionString"];
        ConnectionString = connectionString;
    }

    public string ConnectionString { get; }
}