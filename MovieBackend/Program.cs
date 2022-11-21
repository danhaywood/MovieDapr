using System.Reflection;
using EvolveDb;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MovieBackend.Controllers;
using MovieBackend.Data;
using MovieBackend.Graphql;
using MovieBackend.Models;
using MovieFrontend.Sql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
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
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
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
builder.Services.AddDaprSidekick(builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("MovieBackendContext") ??
                             throw new InvalidOperationException( "Connection string 'MovieBackendContext' not found.");

builder.Services.AddDbContext<MovieDbContext>(options =>
// builder.Services.AddPooledDbContextFactory<MovieDbContext>(options =>
{
    options.UseLazyLoadingProxies()
            .UseSqlServer(connectionString);
});

var app = builder.Build();

var isDevelopment = app.Environment.IsDevelopment();

var postBuild = Environment.GetEnvironmentVariable("POST_BUILD");
if (postBuild == null)
{
    var evolve = new Evolve(new SqlConnection(connectionString), logDelegate: s => Console.Out.WriteLine("Evolve: " + s))
    {
        EmbeddedResourceAssemblies = new[] { typeof(Sql).Assembly },
        IsEraseDisabled = !isDevelopment,
        RetryRepeatableMigrationsUntilNoError = true,
        MustEraseOnValidationError = isDevelopment
    };
    evolve.Migrate();
    
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        SeedData.Initialize(services);
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

app.Run();
