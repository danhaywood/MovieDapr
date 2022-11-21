using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MovieBackend.Controllers;
using MovieBackend.Data;
using MovieData;

namespace MovieBackend.Models
{
    public class MovieRepository
    {
        private static readonly ActivitySource ActivitySource = new(nameof(MovieRepository));
        
        private readonly MovieContext _context;
        public MovieRepository(MovieContext context)
        {
            _context = context;
        }

        public DbSet<Movie> GetMovies()
        {
            using (ActivitySource.StartActivity(nameof(GetMovies), ActivityKind.Client))
            {
                return _context.Movie;
            }
        }
        
        public async Task<List<Movie>> GetMoviesAsync()
        {
            using (ActivitySource.StartActivity(nameof(GetMoviesAsync), ActivityKind.Client))
            {
                return await _context.Movie.ToListAsync();
            }
        }
        
        public async Task<Movie?> GetMovieAsync(int id)
        {
            using (ActivitySource.StartActivity(nameof(GetMovieAsync), ActivityKind.Client))
            {
                return await _context.Movie.FindAsync(id);
            }
        }
        
        public async Task<Movie> CreateMovieAsync(MovieDto movieDto)
        {
            var movie = new Movie(movieDto);
            using (ActivitySource.StartActivity(nameof(CreateMovieAsync), ActivityKind.Client))
            {
                var movieEntry = _context.Movie.Add(movie);
                await _context.SaveChangesAsync();
                return movieEntry.Entity;
            }
        }
        
        public async Task<Movie?> UpdateMovieAsync(MovieDto movieDto)
        {
            using (ActivitySource.StartActivity(nameof(UpdateMovieAsync), ActivityKind.Client))
            {
                Movie? movie;
                int id;
                
                using (ActivitySource.StartActivity(nameof(UpdateMovieAsync) + ".Find", ActivityKind.Client))
                {
                    id = movieDto.ID;
        
                    movie = await _context.Movie.FindAsync(id);
                    if (movie == null)
                    {
                        return null;
                    }
                }
                
                movie.Genre = movieDto.Genre;
                movie.Title = movieDto.Title;
                movie.Price = movieDto.Price;
                movie.ReleaseDate = movieDto.ReleaseDate;
                
                using (ActivitySource.StartActivity(nameof(UpdateMovieAsync) + ".Save", ActivityKind.Client))
                {
                    _context.Attach(movie).State = EntityState.Modified;
        
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException) when (!MovieExists(id))
                    {
                        return null;
                    }
        
                    return await GetMovieAsync(id);
                }
            }
        }
        
        public async Task<bool> DeleteMovieAsync(int id)
        {
        
            using (ActivitySource.StartActivity(nameof(DeleteMovieAsync), ActivityKind.Client))
            {
                
                Movie? movie;
                using (ActivitySource.StartActivity(nameof(DeleteMovieAsync) + ".Find", ActivityKind.Client))
                {
                    movie = await _context.Movie.FindAsync(id);
                    if (movie == null)
                    {
                        return false;
                    }
                }
                using (ActivitySource.StartActivity(nameof(DeleteMovieAsync) + ".Delete", ActivityKind.Client))
                {
                    _context.Movie.Remove(movie);
                    await _context.SaveChangesAsync();
        
                    return true;
                }
            }
        }
        
        private bool MovieExists(int id)
        {
            using (ActivitySource.StartActivity(nameof(MovieExists), ActivityKind.Client))
            {
                return _context.Movie.Any(x => x.ID == id);
            }
        }


        public async Task<Movie?> FindMovieByTitleAsync(string title)
        {
            using (ActivitySource.StartActivity(nameof(FindMovieByTitleAsync), ActivityKind.Client))
            {
                return await _context.Movie.FirstAsync(x => x.Title == title);
            }
        }
    }
}