using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBackend.Data;
using MovieBackend.Models;
using MovieData;

namespace MovieBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieRepository _movieRepository;

        public MoviesController(MovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _movieRepository.GetMoviesAsync();
            return movies.Select(x => x.AsDto()).ToList();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await  _movieRepository.GetMovieAsync(id);
            return movie == null ? NotFound() : movie.AsDto();
        }
        
        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieDto>> CreateMovie(MovieDto movieDto)
        {
            var createdMovie = await _movieRepository.CreateMovieAsync(movieDto);
            return CreatedAtAction(
                nameof(CreateMovie),
                new { id = createdMovie.Id },
                createdMovie.AsDto());
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<MovieDto>> UpdateMovie(MovieDto movieDto)
        {
            var updatedMovie = await _movieRepository.UpdateMovieAsync(movieDto);
            return updatedMovie == null ? NotFound() : Ok(updatedMovie.AsDto());
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
            var result =  await _movieRepository.DeleteMovieAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}