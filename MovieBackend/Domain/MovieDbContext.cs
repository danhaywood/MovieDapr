using Microsoft.EntityFrameworkCore;
using MovieBackend.Models;

namespace MovieBackend.Data
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext (DbContextOptions<MovieDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; } = default!;
        public DbSet<Actor> Actor { get; set; } = default!;
        public DbSet<Character> Character { get; set; } = default!;
        
    }
}
