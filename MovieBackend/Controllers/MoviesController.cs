using System.Diagnostics;
using Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBackend.Data;
using MovieBackend.Models;

namespace MovieBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MoviesController : ControllerBase
    {
        private static readonly ActivitySource ActivitySource = new(nameof(MoviesController));
        
        private readonly MovieContext _context;
        
        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            using (ActivitySource.StartActivity(nameof(GetMovies), ActivityKind.Client))
            {
                return await _context.Movie.Select(x => x.AsDto()).ToListAsync();
            }
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            using (ActivitySource.StartActivity(nameof(GetMovie), ActivityKind.Client))
            {
                var movie = await _context.Movie.FindAsync(id);
                return movie == null ? NotFound() : movie.AsDto();
            }
        }
        
        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieDto>> CreateMovie(MovieDto movieDto)
        {
            using (ActivitySource.StartActivity(nameof(CreateMovie), ActivityKind.Client))
            {
                var movie = new Movie(movieDto);
                _context.Movie.Add(movie);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(CreateMovie),
                    new { id = movie.ID },
                    movie.AsDto());
            }
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<MovieDto>> UpdateMovie(MovieDto movieDto)
        {
            using (ActivitySource.StartActivity(nameof(UpdateMovie), ActivityKind.Client))
            {
                Movie? movie;
                int id;
                
                using (ActivitySource.StartActivity(nameof(UpdateMovie) + ".Find", ActivityKind.Client))
                {
                    id = movieDto.ID;

                    movie = await _context.Movie.FindAsync(id);
                    if (movie == null)
                    {
                        return NotFound();
                    }
                }
                
                movie.Genre = movieDto.Genre;
                movie.Title = movieDto.Title;
                movie.Price = movieDto.Price;
                movie.ReleaseDate = movieDto.ReleaseDate;
                
                using (ActivitySource.StartActivity(nameof(UpdateMovie) + ".Save", ActivityKind.Client))
                {
                    _context.Attach(movie).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException) when (!MovieExists(id))
                    {
                        return NotFound();
                    }

                    return await GetMovie(id);
                }
            }
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
        public async Task<IActionResult> DeleteMovie(int id)
        {

            using (ActivitySource.StartActivity(nameof(DeleteMovie), ActivityKind.Client))
            {
                
                Movie? movie;
                using (ActivitySource.StartActivity(nameof(DeleteMovie) + ".Find", ActivityKind.Client))
                {
                    movie = await _context.Movie.FindAsync(id);

                    if (movie == null)
                    {
                        return NotFound();
                    }
                }
                using (ActivitySource.StartActivity(nameof(DeleteMovie) + ".Delete", ActivityKind.Client))
                {
                    _context.Movie.Remove(movie);
                    await _context.SaveChangesAsync();

                    return NoContent();
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
    }
}