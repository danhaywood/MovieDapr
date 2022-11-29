namespace MovieBackend.Infra.Bootstrap;

public class ScopedBootstrapper<T> : IHostedService where T: IBootstrappable 
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IServiceProvider _serviceProvider;

    public ScopedBootstrapper(IHostApplicationLifetime hostApplicationLifetime, IServiceProvider serviceProvider)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void OnStarted()
    {
        using (var scope = _serviceProvider.CreateScope()) {
            var seedDataService = scope.ServiceProvider.GetService<T>();
            seedDataService!.Bootstrap();
        }
    }
}