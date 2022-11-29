using Dapr.Client;
using Man.Dapr.Sidekick.Http;
using Microsoft.EntityFrameworkCore;
using MovieBackend.Graphql;
using MovieBackend.Models;

namespace MovieBackend.Graphql
{
    public class MovieDataDbContext : DbContext
    {
        public MovieDataDbContext (DbContextOptions<MovieDataDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var daprClient = new DaprClientBuilder().Build();
            var secretAsync = daprClient.GetSecretAsync("movie-secret-store", "ConnectionString");
            var connectionString = secretAsync.Result["ConnectionString"];
            
            optionsBuilder.UseLazyLoadingProxies() .UseSqlServer( connectionString);
        }

        public DbSet<MovieData> MovieData { get; set; } = default!;
        public DbSet<ActorData> ActorData { get; set; } = default!;
        public DbSet<CharacterData> CharacterData { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("graphql");
            
            modelBuilder.Entity<MovieData>()    .ToView("MovieData");
            modelBuilder.Entity<ActorData>()    .ToView("ActorData");
            modelBuilder.Entity<CharacterData>().ToView("CharacterData");
        }
    }
}
