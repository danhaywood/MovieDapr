using System.Reflection;
using Microsoft.OpenApi.Models;
using MovieBackend.Domain;
using MovieBackend.Domain.Seed;
using MovieBackend.Graphql.Migrate;
using MovieBackend.Infra.Bootstrap;
using MovieBackend.Infra.ConnStr;
using MovieBackend.Models;
using MovieBackend.Read;
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


// distributed tracing 
builder.Services.AddOpenTelemetryTracing(options =>
{
    options
        .ConfigureResource(r => r.AddService( serviceName: assemblyName + " app", serviceVersion: assemblyVersion, serviceInstanceId: Environment.MachineName))
        .AddSource(nameof(MovieRepository))         // explicit ActivitySource
        .AddSource(nameof(ActorRepository))         // explicit ActivitySource
        .AddSource(nameof(CharacterRepository))     // explicit ActivitySource
        .SetSampler(new AlwaysOnSampler())
        .AddAspNetCoreInstrumentation()
        .AddSqlClientInstrumentation(options =>     // trace SQL calls
        {
            options.SetDbStatementForText = true;
            options.RecordException = true;
            options.SetDbStatementForStoredProcedure = true;
        })
        //.AddHttpClientInstrumentation()   // don't do this, otherwise we'll instrument our calls to the dapr sidecar.
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

// builder.Services.AddPooledDbContextFactory<MovieDbContext>(optionsBuilder => {});
// builder.Services.AddPooledDbContextFactory<MovieViewDbContext>(optionsBuilder => {});
builder.Services.AddDbContextFactory<MovieDbContext>();
builder.Services.AddDbContextFactory<MovieViewDbContext>();
builder.Services.AddScoped<MovieRepository>();
builder.Services.AddScoped<ActorRepository>();
builder.Services.AddScoped<CharacterRepository>();


builder.Services.Configure<AspNetCoreInstrumentationOptions>(builder.Configuration.GetSection("AspNetCoreInstrumentation"));

builder.Services.AddControllers();
// builder.Services.AddControllers().AddDapr(); // not necessary, think this is to support pubsub attributes, eg [Topic]

// swagger support
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Movie API",
        Description = "An ASP.NET Core Web API for managing Movie items",
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

// graphql support
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    ;

// builder.Services.AddRazorPages();

if (Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") != null)
{
    // running under an orchestrator    
}
else
{
    // local environment, start up our own daprd
    builder.Services.AddDaprSidekick(builder.Configuration);
}

builder.Services.AddSingleton<IConnectionStringService, ConnectionStringServiceUsingDaprSecrets>();

var postBuild = Environment.GetEnvironmentVariable("POST_BUILD");
if (postBuild == null)
{
    builder.Services.AddScoped<EvolveMigrateService>();
    builder.Services.AddHostedService<ScopedBootstrapper<EvolveMigrateService>>();
    
    builder.Services.AddScoped<SeedMoviesService>();
    builder.Services.AddHostedService<ScopedBootstrapper<SeedMoviesService>>();
}

var app = builder.Build();


// Configure the HTTP request pipeline.
var isDevelopment = app.Environment.IsDevelopment();
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
