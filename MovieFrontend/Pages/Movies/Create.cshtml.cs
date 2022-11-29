using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieFrontend.PageBindingModels;
using MovieFrontend.Update;

namespace MovieFrontend.Pages.Movies;

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
    public MoviePbm Movie { get; set; }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _moviesService.CreateMovie(Movie.AsDto());

        return RedirectToPage("./Index");
    }
}