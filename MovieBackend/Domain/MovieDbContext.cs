using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using MovieBackend.Models;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace MovieBackend.Domain
{
    //[DbConfigurationType(typeof(MyDbConfiguration))]    
    public class MovieDbContext : DbContext
    {
        public MovieDbContext (DbContextOptions<MovieDbContext> options)
            : base(options)
        {
        }

        public Microsoft.EntityFrameworkCore.DbSet<Movie> Movie { get; set; } = default!;
        public Microsoft.EntityFrameworkCore.DbSet<Actor> Actor { get; set; } = default!;
        public Microsoft.EntityFrameworkCore.DbSet<Character> Character { get; set; } = default!;
    }

    public class MyDbConfiguration : DbConfiguration
    {
        public MyDbConfiguration()
        {
            
        }
    }
}
