using MovieFrontend.Infra.DaprSidecar;
using MovieFrontend.Infra.Http;
using MovieFrontend.Update;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddScoped<MoviesService, MoviesService>();

if (Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") != null)
{
    // running under an orchestrator    
    builder.Services.AddTransient<IDaprHttpPortService, DaprHttpPortServiceUsingEnvVar>();
}
else
{
    // local environment, start up our own daprd
    builder.Services.AddDaprSidekick(builder.Configuration);
    builder.Services.AddTransient<IDaprHttpPortService, DaprHttpPortServiceUsingSidekick>();
}

builder.Services.AddHttpClient();                                                   // install DefaultHttpClientFactory
builder.Services.AddSingleton<IHttpClientFactory, DaprAwareHttpClientFactory>();    // decorator that sets up dapr headers

builder.Services.AddMovieBackendGraphqlClient() ;

var app = builder.Build();


// Configure the HTTP request pipeline.
var isDevelopment = app.Environment.IsDevelopment();
if (isDevelopment)
{
    app.UseDeveloperExceptionPage();
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
app.MapRazorPages();
app.MapControllers();

app.Run();
