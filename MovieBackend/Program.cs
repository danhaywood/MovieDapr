using System.Reflection;
using EvolveDb;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MovieFrontend;
using MovieFrontend.Data;
using MovieFrontend.Models;
using MovieFrontend.Services;
using MovieFrontend.Sql;
using RazorPagesMovie.Services;
using zipkin4net;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;
using ILogger = zipkin4net.ILogger;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<TraceService, TraceService>();
builder.Services.AddHostedService<LifetimeService>();

var connectionString = builder.Configuration.GetConnectionString("RazorPagesMovieContext") ??
                             throw new InvalidOperationException( "Connection string 'RazorPagesMovieContext' not found.");

builder.Services.AddDbContext<MovieContext>(options =>
{
    options.UseSqlServer(connectionString);
});



IZipkinSender sender = new HttpZipkinSender("http://localhost:9411", "text/plain");

TraceManager.SamplingRate = 1.0f; //full tracing

var tracer = new ZipkinTracer(sender, new ThriftSpanSerializer());
TraceManager.RegisterTracer(tracer);
TraceManager.Start(new ZipkinClientLogger());

//Run your application

//On shutdown
// TraceManager.Stop();

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
