using Dapr.Client;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var daprClient = new DaprClientBuilder().Build();
            var secretAsync = daprClient.GetSecretAsync("movie-secret-store", "ConnectionString");
            var connectionString = secretAsync.Result["ConnectionString"];
            
            optionsBuilder.UseLazyLoadingProxies() .UseSqlServer(connectionString);
        }

        public DbSet<Movie> Movie { get; set; } = default!;
        public DbSet<Actor> Actor { get; set; } = default!;
        public DbSet<Character> Character { get; set; } = default!;
        
    }
}
