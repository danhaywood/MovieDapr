using EvolveDb;
using Microsoft.Data.SqlClient;
using MovieFrontend.Sql;

namespace MovieBackend.Services;

public class EvolveMigrateService : IBootstrappable
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ConnectionStringService _connectionStringService;

    public EvolveMigrateService(IWebHostEnvironment webHostEnvironment, ConnectionStringService connectionStringService)
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
            EmbeddedResourceAssemblies = new[] { typeof(Sql).Assembly },
            IsEraseDisabled = !isDevelopment,
            RetryRepeatableMigrationsUntilNoError = true,
            MustEraseOnValidationError = isDevelopment
        };
        evolve.Migrate();

    }

}