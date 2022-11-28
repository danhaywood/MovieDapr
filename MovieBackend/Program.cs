using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using Dapr.Client;
using EvolveDb;
using Man.Dapr.Sidekick;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MovieBackend.Domain;
using MovieBackend.Graphql;
using MovieBackend.Models;
using MovieFrontend.Sql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Path = System.IO.Path;

var builder = WebApplication.CreateBuilder(args);

// OpenTelemetry
var assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "unknown";
var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";

// Switch between Zipkin/<!--Jaeger/OTLP-->/Console by setting UseTracingExporter in appsettings.json.
var tracingExporter = builder.Configuration.GetValue<string>("UseTracingExporter").ToLowerInvariant();


builder.Services.AddOpenTelemetryTracing(options =>
{
    options
        .ConfigureResource(r => r.AddService( serviceName: assemblyName + " app", serviceVersion: assemblyVersion, serviceInstanceId: Environment.MachineName))
        .AddSource(nameof(MovieRepository))
        .AddSource(nameof(ActorRepository))
        .AddSource(nameof(CharacterRepository))
        .SetSampler(new AlwaysOnSampler())
        //.AddHttpClientInstrumentation()   // don't do this, otherwise we'll instrument our calls to the dapr sidecar.
        .AddAspNetCoreInstrumentation()
        .AddSqlClientInstrumentation(options =>
        {
            options.SetDbStatementForText = true;
            options.RecordException = true;
            options.SetDbStatementForStoredProcedure = true;
        })
        ;

    switch (tracingExporter)
    {
        case "zipkin":
            options.AddZipkinExporter();

            builder.Services.Configure<ZipkinExporterOptions>(builder.Configuration.GetSection("Zipkin"));
            break;

        default:
            options.AddConsoleExporter();

            break;
    }
});

// domain repositories
builder.Services.AddScoped<MovieRepository>();
builder.Services.AddScoped<ActorRepository>();
builder.Services.AddScoped<CharacterRepository>();

builder.Services.Configure<AspNetCoreInstrumentationOptions>(builder.Configuration.GetSection("AspNetCoreInstrumentation"));

// Add services to the container.
builder.Services.AddControllers();
// builder.Services.AddControllers().AddDapr(); // not necessary, think this is to support pubsub attributes, eg [Topic]


//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Movie App API",
        Description = "An ASP.NET Core Web API for exploring movies",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    ;

builder.Services.AddRazorPages();
builder.Services.AddDaprSidekick(builder.Configuration, options =>
{
    Console.WriteLine("hi there");
});


var postBuild = Environment.GetEnvironmentVariable("POST_BUILD");
if (postBuild == null)
{

    var connectionString = builder.Configuration.GetConnectionString("MovieBackendContext") ??
                                 throw new InvalidOperationException( "Connection string 'MovieBackendContext' not found.");

    builder.Services.AddDbContext<MovieDbContext>(options =>
    {
        options.UseLazyLoadingProxies().UseSqlServer(connectionString);
    });

    builder.Services.AddDbContext<MovieDataDbContext>(options =>
    // builder.Services.AddPooledDbContextFactory<MovieDataDbContext>(options =>
    {
        options.UseLazyLoadingProxies().UseSqlServer(connectionString);
    });

    builder.Services.AddHostedService<RunEvolveMigrate>();
}


WebApplication app = builder.Build();

var isDevelopment = app.Environment.IsDevelopment();

DbConfiguration.Loaded += (_, a) =>
{
    Console.WriteLine(a);
    // a.ReplaceService<DbProviderServices>((s, k) => new MyProviderServices(s));
    // a.ReplaceService<IDbConnectionFactory>((s, k) => new MyConnectionFactory(s));
};

if (postBuild == null)
{
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        SeedData.Initialize(serviceProvider);
    }
}

// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else 
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.MapGraphQL();
app.MapHealthChecks("/health");
app.MapDaprMetrics("/metrics");

app.Run();


public class RunEvolveMigrate : IHostedService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IServiceProvider _serviceProvider;

    public RunEvolveMigrate(IWebHostEnvironment webHostEnvironment, IServiceProvider serviceProvider)
    {
        _webHostEnvironment = webHostEnvironment;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var isDevelopment = _webHostEnvironment.IsDevelopment();
        // var connectionString =  builder.Configuration.GetConnectionString("MovieBackendContext") ??
        //                        throw new InvalidOperationException( "Connection string 'MovieBackendContext' not found.");

        var connectionString =
            "Server=halxps15-2022\\SQLEXPRESS;Database=dbMovie;Integrated Security=True;Encrypt=False;Trusted_Connection=True;MultipleActiveResultSets=true";

        // var baseURL = (Environment.GetEnvironmentVariable("BASE_URL") ?? "http://localhost") + ":" 
        //     + (Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3500");
        // const string DAPR_SECRET_STORE = "localsecretstore";
        // const string SECRET_NAME = "secretFile";
        //
        // var httpClient = new HttpClient();
        // httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //
        // var secret = await httpClient.GetStringAsync($"{baseURL}/v1.0/secrets/{DAPR_SECRET_STORE}/{SECRET_NAME}");
        // Console.WriteLine("Fetched Secret: " + secret);

    
        var evolve = new Evolve(new SqlConnection(connectionString), logDelegate: s => Console.Out.WriteLine("Evolve: " + s))
        {
            EmbeddedResourceAssemblies = new[] { typeof(Sql).Assembly },
            IsEraseDisabled = !isDevelopment,
            RetryRepeatableMigrationsUntilNoError = true,
            MustEraseOnValidationError = isDevelopment
        };
        evolve.Migrate();
    
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        //Cleanup logic here
    }
}
