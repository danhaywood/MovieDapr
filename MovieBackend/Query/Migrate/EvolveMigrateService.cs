using EvolveDb;
using Microsoft.Data.SqlClient;
using MovieBackend.Infra.Bootstrap;
using MovieBackend.Infra.ConnStr;

namespace MovieBackend.Graphql.Migrate;

public class EvolveMigrateService : IBootstrappable
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IConnectionStringService _connectionStringService;

    public EvolveMigrateService(IWebHostEnvironment webHostEnvironment, IConnectionStringService connectionStringService)
    {
        _webHostEnvironment = webHostEnvironment;
        _connectionStringService = connectionStringService;
    }

    public void Bootstrap()
    {
        var isDevelopment = _webHostEnvironment.IsDevelopment();

        var connectionString = _connectionStringService.ConnectionString;
        var evolve = new Evolve(new SqlConnection(connectionString), logDelegate: s => Console.Out.WriteLine("Evolve: " + s))
        {
            EmbeddedResourceAssemblies = new[] { typeof(EvolveMigrateService).Assembly },
            IsEraseDisabled = !isDevelopment,
            RetryRepeatableMigrationsUntilNoError = true,
            MustEraseOnValidationError = isDevelopment
        };
        evolve.Migrate();

    }

}