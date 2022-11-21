using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieData;
using MovieFrontend.Services;

namespace MovieFrontend.Views.Movies
{
    public class IndexModel : PageModel
    {
        private readonly MoviesService _moviesService;
        public IndexModel(MoviesService moviesService)
        {
            _moviesService = moviesService;
        }

        public IList<MovieDto> Movie { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string ? SearchString { get; set; }
        public SelectList ? Genres { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ? MovieGenre { get; set; }
        
        public async Task OnGetAsync()
        {
            Movie = await _moviesService.GetMovies();  // no search just yet...  _context.Movie.Where(s => string.IsNullOrEmpty(SearchString) || s.Title.Contains(SearchString)).Select(x => x.AsDto());
        }
        
    }
}
