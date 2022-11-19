using EvolveDb;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.DependencyInjection;
using RazorPagesMovie;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;
using RazorPagesMovie.Sql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("RazorPagesMovieContext") ??
                             throw new InvalidOperationException( "Connection string 'RazorPagesMovieContext' not found.");


builder.Services.AddDbContext<RazorPagesMovieContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

var isProdEnv = !app.Environment.IsDevelopment();
var evolve = new Evolve(new SqlConnection(connectionString), logDelegate: s => Console.Out.WriteLine("Evolve: " + s))
{
    EmbeddedResourceAssemblies = new[] { typeof(Sql).Assembly },
    IsEraseDisabled = isProdEnv,
    MustEraseOnValidationError = isProdEnv
};
evolve.Migrate();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}


// Configure the HTTP request pipeline.
if (isProdEnv)
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
