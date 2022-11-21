using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieData;
using MovieFrontend.Services;

namespace MovieFrontend.Views.Movies
{
    public class DeleteModel : PageModel
    {
        private readonly MoviesService _moviesService;
        public DeleteModel(MoviesService moviesService)
        {
            _moviesService = moviesService;
        }

    [BindProperty]
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
