using Microsoft.EntityFrameworkCore;
using MovieFrontend.Models;

namespace MovieFrontend.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext (DbContextOptions<MovieContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; } = default!;
    }
}
