using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieClient;
using MovieFrontend.PageBindingModels;
using StrawberryShake;

namespace MovieFrontend.Pages.Movies;

public class DetailsModel : PageModel
{
    private readonly IMovieBackendGraphqlClient _backendGraphqlClient;
    public DetailsModel(IMovieBackendGraphqlClient backendGraphqlClient)
    {
        _backendGraphqlClient = backendGraphqlClient;
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
}