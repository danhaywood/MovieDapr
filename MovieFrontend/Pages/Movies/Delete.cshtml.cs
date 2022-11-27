using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieFrontend.PageBindingModels;
using MovieFrontend.Services;
using StrawberryShake;

namespace MovieFrontend.Pages.Movies
{
    public class DeleteModel : PageModel
    {
        private readonly IMovieBackendGraphqlClient _backendGraphqlClient;
        private readonly MoviesService _moviesService;
        public DeleteModel(IMovieBackendGraphqlClient backendGraphqlClient, MoviesService moviesService)
        {
            _backendGraphqlClient = backendGraphqlClient;
            _moviesService = moviesService;
        }

        [BindProperty]
        public MoviePbm Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _backendGraphqlClient.Movie_by_id.ExecuteAsync(id.Value);
            movie.EnsureNoErrors();
            if (movie.Data == null)
            {
                return NotFound();
            }

            Movie = movie.Data.Movies.Select(x => x.AsPbm()).First();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await _moviesService.DeleteMovie(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
