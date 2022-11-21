using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieData;
using MovieFrontend.Services;

namespace MovieFrontend.Views.Movies
{
    public class CreateModel : PageModel
    {
        private readonly MoviesService _moviesService;
        public CreateModel(MoviesService moviesService)
        {
            _moviesService = moviesService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public MovieDto Movie { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
          {
              return Page();
          }

          await _moviesService.CreateMovie(Movie);

          return RedirectToPage("./Index");
        }
    }
}
