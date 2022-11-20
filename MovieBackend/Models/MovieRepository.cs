using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MovieBackend.Controllers;
using MovieBackend.Data;
using MovieData;

namespace MovieBackend.Models
{
    public class MovieRepository
    {
        private static readonly ActivitySource ActivitySource = new(nameof(MoviesController));
        
        private readonly MovieContext _context;
        
        public MovieRepository(MovieContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        public async Task<List<MovieDto>> GetMovies()
        {
            using (ActivitySource.StartActivity(nameof(GetMovies), ActivityKind.Client))
            {
                Task<List<MovieDto>> listAsync = _context.Movie.Select(x => x.AsDto()).ToListAsync();
                return await listAsync;
            }
        }
    }
}