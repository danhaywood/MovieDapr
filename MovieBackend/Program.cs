﻿using System.Reflection;
using EvolveDb;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MovieBackend.Data;
using MovieBackend.Models;
using MovieFrontend.Sql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// OpenTelemetry
var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";

// Switch between Zipkin/<!--Jaeger/OTLP-->/Console by setting UseTracingExporter in appsettings.json.
var tracingExporter = builder.Configuration.GetValue<string>("UseTracingExporter").ToLowerInvariant();

var serviceName = tracingExporter switch
{
    // "jaeger" => builder.Configuration.GetValue<string>("Jaeger:ServiceName"),
    "zipkin" => builder.Configuration.GetValue<string>("Zipkin:ServiceName"),
    // "otlp" => builder.Configuration.GetValue<string>("Otlp:ServiceName"),
    _ => "AspNetCoreExampleService",
};

Action<ResourceBuilder> configureResource = r => r.AddService(
    serviceName, serviceVersion: assemblyVersion, serviceInstanceId: Environment.MachineName);

builder.Services.AddOpenTelemetryTracing(options =>
{
    options
        .ConfigureResource(configureResource)
        .SetSampler(new AlwaysOnSampler())
        //.AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        ;

    switch (tracingExporter)
    {
        // case "jaeger":
        //     options.AddJaegerExporter();
        //
        //     builder.Services.Configure<JaegerExporterOptions>(builder.Configuration.GetSection("Jaeger"));
        //
        //     // Customize the HttpClient that will be used when JaegerExporter is configured for HTTP transport.
        //     builder.Services.AddHttpClient("JaegerExporter", configureClient: (client) => client.DefaultRequestHeaders.Add("X-MyCustomHeader", "value"));
        //     break;

        case "zipkin":
            options.AddZipkinExporter();

            builder.Services.Configure<ZipkinExporterOptions>(builder.Configuration.GetSection("Zipkin"));
            break;

        // case "otlp":
        //     options.AddOtlpExporter(otlpOptions =>
        //     {
        //         otlpOptions.Endpoint = new Uri(builder.Configuration.GetValue<string>("Otlp:Endpoint"));
        //     });
        //     break;

        default:
            options.AddConsoleExporter();

            break;
    }
});

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

builder.Services.AddRazorPages();
builder.Services.AddDaprSidekick(builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("RazorPagesMovieContext") ??
                             throw new InvalidOperationException( "Connection string 'RazorPagesMovieContext' not found.");

builder.Services.AddDbContext<MovieContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

var isDevelopment = app.Environment.IsDevelopment();
var evolve = new Evolve(new SqlConnection(connectionString), logDelegate: s => Console.Out.WriteLine("Evolve: " + s))
{
    EmbeddedResourceAssemblies = new[] { typeof(Sql).Assembly },
    IsEraseDisabled = !isDevelopment,
    MustEraseOnValidationError = !isDevelopment
};
evolve.Migrate();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
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

app.Run();
