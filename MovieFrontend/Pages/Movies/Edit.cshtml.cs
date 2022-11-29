using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieClient;
using MovieData;
using MovieFrontend.PageBindingModels;
using MovieFrontend.Update;
using StrawberryShake;

namespace MovieFrontend.Pages.Movies;

public class EditModel : PageModel
{
    private readonly IMovieBackendGraphqlClient _backendGraphqlClient;
    private readonly MoviesService _moviesService;

    public EditModel(IMovieBackendGraphqlClient backendGraphqlClient, MoviesService moviesService)
    {
        _backendGraphqlClient = backendGraphqlClient;
        _moviesService = moviesService;
    }

    [BindProperty]
    public MoviePbm Movie { get; set; } = null!;

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

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var movie = await _moviesService.UpdateMovie(Movie.Id, Movie.AsDto());
        if (movie == null)
        {
            return NotFound();
        }

        return RedirectToPage("./Index");
    }

}