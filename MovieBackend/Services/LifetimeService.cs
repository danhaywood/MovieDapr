using zipkin4net;

namespace MovieFrontend.Services
{
    public class LifetimeService : IHostedService
    {
        private readonly ILogger<LifetimeService> _logger;
        private readonly IHostApplicationLifetime _appLifetime;

        public LifetimeService(ILogger<LifetimeService> logger, IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _appLifetime = appLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;   
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        
        private void OnStarted()
        {
        }

        private void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");
            TraceManager.Stop();
        }
        
        private void OnStopped()
        {
        }
    }
}