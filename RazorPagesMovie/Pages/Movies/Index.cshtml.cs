using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Api;

namespace RazorPagesMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IList<MovieDto> Movie { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string ? SearchString { get; set; }
        public SelectList ? Genres { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ? MovieGenre { get; set; }
        
        public async Task OnGetAsync()
        {
            var movies = _context.Movie.Where(s => string.IsNullOrEmpty(SearchString) || s.Title.Contains(SearchString)).Select(x => x.AsDto());
            Movie = await movies.ToListAsync();
        }
        
    }
}
