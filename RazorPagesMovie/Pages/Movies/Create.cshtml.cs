using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class CreateModel : PageModel
    {
        private readonly RazorPagesMovieContext _context;

        public CreateModel(RazorPagesMovieContext context)
        {
            _context = context;
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

          _context.Movie.Add(new Movie(Movie));
          await _context.SaveChangesAsync();

          return RedirectToPage("./Index");
        }
    }
}
