using Dapr.Client;
using MovieBackend.Graphql;

namespace MovieBackend.Services;

public class ConnectionStringService 
{
    public String GetConnectionString()
    {
        var daprClient = new DaprClientBuilder().Build();
        var secretAsync = daprClient.GetSecretAsync("movie-secret-store", "ConnectionString");
        var connectionString = secretAsync.Result["ConnectionString"];
        return connectionString;
    }
}