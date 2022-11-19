using Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MoviesController : ControllerBase
    {
        private readonly Data.RazorPagesMovieContext _context;
        public MoviesController(Data.RazorPagesMovieContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            return await _context.Movie.Select(x => x.AsDto()).ToListAsync();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(long id)
        {
            var todoItem = await _context.Movie.FindAsync(id);
            return todoItem == null ? NotFound() : todoItem.AsDto();
        }
        
        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, MovieDto movieDto)
        {
            if (id != movieDto.ID)
            {
                return BadRequest();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            
            movie.Genre = movieDto.Genre;
            movie.Title = movieDto.Title;
            movie.Price = movieDto.Price;
            movie.ReleaseDate = movieDto.ReleaseDate;

            _context.Attach(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!MovieExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieDto>> CreateMovie(MovieDto movieDto)
        {
            var movie = new Movie(movieDto);
            _context.Movie.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetMovie),
                new { id = movie.ID },
                movie.AsDto());
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
        public async Task<IActionResult> DeleteMovie(long id)
        {
            var todoItem = await _context.Movie.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Movie.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(long id)
        {
            return _context.Movie.Any(x => x.ID == id);
        }

    }
}