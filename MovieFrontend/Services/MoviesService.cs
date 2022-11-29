﻿using MovieData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dapr.Client;
using Man.Dapr.Sidekick.Http;
using Man.Dapr.Sidekick.Security;
using MovieFrontend.PageBindingModels;
using StrawberryShake;

namespace MovieFrontend.Services;

public class MoviesService
{
    private readonly HttpClient _httpClient;
    private readonly MovieBackendGraphqlClient _movieBackendGraphqlClient;

    public MoviesService(IDaprSidecarHttpClientFactory daprSidecarHttpClientFactory, MovieBackendGraphqlClient movieBackendGraphqlClient)
    {
        _movieBackendGraphqlClient = movieBackendGraphqlClient;
        var httpClient = daprSidecarHttpClientFactory.CreateInvokeHttpClient("moviebackend");
        // var httpClient = new HttpClient();
        // httpClient.DefaultRequestHeaders.Add("dapr-app-id", "moviebackend");
        // httpClient.BaseAddress = new Uri("http://localhost:3500");   // seems to work irrespective of which app is started first

        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            
        _httpClient = httpClient;
    }

    // GET: api/Movies
    [HttpGet]
    public async Task<List<MovieDto>> GetMovies()
    {
        var result = await _movieBackendGraphqlClient.Movies.ExecuteAsync();
        // probably wouldn't bother to marshall to a DTO, the result.Data already is our DTO structure.
        var movieDtos = result.Data?.Movies.Select(x => x.ToDto()).ToList();
        return movieDtos ?? new List<MovieDto>();

        // var response = await _httpClient.GetAsync("/api/Movies");
        // response.EnsureSuccessStatusCode();
        //
        // return await response.Content.ReadFromJsonAsync<List<MovieDto>>() ?? new List<MovieDto>();
    }

    // GET: api/Movies/5
    [HttpGet("{id}")]
    public async Task<MovieDto?> GetMovie(int id)
    {
        var result = await _movieBackendGraphqlClient.Movie_by_id.ExecuteAsync(id);
        return result.Data?.Movies.Select(x => x.ToDto()).First();

        // var response = await _httpClient.GetAsync($"/api/Movies/{id}");
        // response.EnsureSuccessStatusCode();
        //
        // return await response.Content.ReadFromJsonAsync<MovieDto>() ?? null;
    }


    // PUT: api/Movies/5
    [HttpPut("{id}")]
    public async Task<MovieDto?> UpdateMovie(int id, MovieDto movieDto)
    {

        var response = await _httpClient.PutAsJsonAsync($"/api/Movies/{id}", movieDto);
        response.EnsureSuccessStatusCode();
            
        // Deserialize the updated product from the response body.
        return await response.Content.ReadFromJsonAsync<MovieDto>() ?? null;
    }

    // POST: api/Movies
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<Uri> CreateMovie(MovieDto movieDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/Movies", movieDto);
        response.EnsureSuccessStatusCode();
            
        return response.Headers.Location!;
    }

    /// <summary>
    /// Deletes a specific Movie
    /// </summary>
    /// <example>
    /// DELETE: api/Movies/5
    /// </example>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<HttpStatusCode> DeleteMovie(int id)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync(
            $"api/movies/{id}");
        return response.StatusCode;
    }

}

static class IMovies_MoviesExtensions
{
    internal static MovieDto ToDto(this IMovies_Movies x)
    {
        return new MovieDto
        {
            Id = x.Id,
            Genre = x.Genre,
            Title = x.Title,
            Price = x.Price,
            ReleaseDate = x.ReleaseDate.DateTime,
            Characters = x.Characters.Select(c => new CharacterDto
            {
                CharacterName = c.CharacterName
            }).ToList()
        };
    }
        
    internal static MovieDto ToDto(this IMovie_by_id_Movies x)
    {
        return new MovieDto
        {
            Id = x.Id,
            Genre = x.Genre,
            Title = x.Title,
            Price = x.Price,
            ReleaseDate = x.ReleaseDate.DateTime,
            Characters = x.Characters.Select(c => new CharacterDto
            {
                CharacterName = c.CharacterName
            }).ToList()
        };
    }
        
}