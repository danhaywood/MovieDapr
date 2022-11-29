using MovieFrontend.Update;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddScoped<MoviesService, MoviesService>();
builder.Services.AddDaprSidekick(builder.Configuration);

builder.Services.AddMovieBackendGraphqlClient()
    .ConfigureHttpClient(httpClient =>
    {
        // TODO: need to figure out how to obtain the 3501 dynamically, from DaprSidekickHost when running locally.
        httpClient.DefaultRequestHeaders.Add("dapr-app-id", "moviebackend");
        httpClient.BaseAddress = new Uri("http://localhost:3501/graphql");
    }
    );

var app = builder.Build();

var isDevelopment = app.Environment.IsDevelopment();

// Configure the HTTP request pipeline.
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
