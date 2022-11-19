using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Api;
using RazorPagesMovie.Services;

namespace RazorPagesMovie.Pages.Movies
{
    public class EditModel : PageModel
    {
        private readonly MoviesService _moviesService;
        public EditModel(MoviesService moviesService)
        {
            _moviesService = moviesService;
        }

        [BindProperty]
        public MovieDto Movie { get; set; } = default!;

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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var movie = await _moviesService.UpdateMovie(Movie.ID, Movie);
            if (movie == null)
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }

    }
}
