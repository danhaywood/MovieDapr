using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Api;
using MovieFrontend.Services;

namespace MovieFrontend.Pages.Movies
{
    public class DetailsModel : PageModel
    {
        private readonly MoviesService _moviesService;
        public DetailsModel(MoviesService moviesService)
        {
            _moviesService = moviesService;
        }

      public MovieDto Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _moviesService.GetMovie(id.Value);
            if (movie == null)
            {
                return NotFound();
            }

            Movie = movie;
            return Page();
        }
    }
}
