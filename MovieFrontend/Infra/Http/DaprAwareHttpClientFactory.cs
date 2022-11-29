using System.Net.Http.Headers;
using MovieFrontend.Infra.DaprSidecar;

namespace MovieFrontend.Infra.Http;

public class DaprAwareHttpClientFactory : IHttpClientFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDaprHttpPortService _daprHttpPortService;

    public DaprAwareHttpClientFactory(IDaprHttpPortService daprHttpPortService, IServiceProvider serviceProvider)
    {
        _daprHttpPortService = daprHttpPortService;
        _serviceProvider = serviceProvider;
    }
    
    public HttpClient CreateClient(string name)
    {
        var httpClientFactories = _serviceProvider.GetServices<IHttpClientFactory>();
        var httpClientFactory = httpClientFactories.FirstOrDefault(x => x != this);
        var httpClient = httpClientFactory.CreateClient();

        var daprHttpPort = _daprHttpPortService.DaprHttpPort;
        httpClient.DefaultRequestHeaders.Add("dapr-app-id", "moviebackend");

        if (name.Contains("Graphql")) 
        {
            // the generated client has a name "MovieBackendGraphqlClient"
            httpClient.BaseAddress = new Uri($"http://localhost:{daprHttpPort}/graphql");
        }
        else
        {
            // we assume REST
            httpClient.BaseAddress = new Uri($"http://localhost:{daprHttpPort}");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        return httpClient;
    }
}