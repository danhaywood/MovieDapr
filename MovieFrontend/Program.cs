﻿using MovieFrontend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().WithRazorPagesRoot("/Views");
builder.Services.AddScoped<MoviesService, MoviesService>();
builder.Services.AddDaprSidekick(builder.Configuration);

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

app.Run();
