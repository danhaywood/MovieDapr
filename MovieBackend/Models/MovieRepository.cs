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
        
        private readonly MovieDbContext _dbContext;
        public MovieRepository(MovieDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<Movie> GetMovies()
        {
            using (ActivitySource.StartActivity(nameof(GetMovies), ActivityKind.Client))
            {
                return _dbContext.Movie;
            }
        }
        
        public async Task<List<Movie>> GetMoviesAsync()
        {
            using (ActivitySource.StartActivity(nameof(GetMoviesAsync), ActivityKind.Client))
            {
                return await _dbContext.Movie.ToListAsync();
            }
        }
        
        public async Task<Movie?> GetMovieAsync(int id)
        {
            using (ActivitySource.StartActivity(nameof(GetMovieAsync), ActivityKind.Client))
            {
                return await _dbContext.Movie.FindAsync(id);
            }
        }
        
        public async Task<Movie> CreateMovieAsync(MovieDto movieDto)
        {
            var movie = new Movie(movieDto);
            using (ActivitySource.StartActivity(nameof(CreateMovieAsync), ActivityKind.Client))
            {
                var movieEntry = _dbContext.Movie.Add(movie);
                await _dbContext.SaveChangesAsync();
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
        
                    movie = await _dbContext.Movie.FindAsync(id);
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
                    _dbContext.Attach(movie).State = EntityState.Modified;
        
                    try
                    {
                        await _dbContext.SaveChangesAsync();
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
                    movie = await _dbContext.Movie.FindAsync(id);
                    if (movie == null)
                    {
                        return false;
                    }
                }
                using (ActivitySource.StartActivity(nameof(DeleteMovieAsync) + ".Delete", ActivityKind.Client))
                {
                    _dbContext.Movie.Remove(movie);
                    await _dbContext.SaveChangesAsync();
        
                    return true;
                }
            }
        }
        
        private bool MovieExists(int id)
        {
            using (ActivitySource.StartActivity(nameof(MovieExists), ActivityKind.Client))
            {
                return _dbContext.Movie.Any(x => x.ID == id);
            }
        }


        public async Task<Movie?> FindMovieByTitleAsync(string title)
        {
            using (ActivitySource.StartActivity(nameof(FindMovieByTitleAsync), ActivityKind.Client))
            {
                return await _dbContext.Movie.FirstAsync(x => x.Title == title);
            }
        }
    }
}