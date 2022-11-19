using Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RazorPagesMovie.Services
{
    public class MoviesService
    {
        private readonly HttpClient _httpClient;

        public MoviesService()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:7092/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            
            _httpClient = httpClient;

        }

        // GET: api/Movies
        [HttpGet]
        public async Task<List<MovieDto>> GetMovies()
        {
            var response = await _httpClient.GetAsync("api/Movies");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<MovieDto>>() ?? new List<MovieDto>();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<MovieDto?> GetMovie(int id)
        {
            var response = await _httpClient.GetAsync($"api/Movies/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<MovieDto>() ?? null;
        }
        
        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<MovieDto?> UpdateMovie(int id, MovieDto movieDto)
        {

            var response = await _httpClient.PutAsJsonAsync($"api/Movies/{id}", movieDto);
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
            
            return response.Headers.Location;
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
}