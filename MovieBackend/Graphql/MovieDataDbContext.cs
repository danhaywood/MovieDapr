using Dapr.Client;
using Man.Dapr.Sidekick.Http;
using Microsoft.EntityFrameworkCore;
using MovieBackend.Graphql;
using MovieBackend.Infra;
using MovieBackend.Infra.ConnStr;
using MovieBackend.Models;

namespace MovieBackend.Graphql;

public class MovieDataDbContext : DbContext
{
    private readonly IConnectionStringService _connectionStringService;

    public MovieDataDbContext (DbContextOptions<MovieDataDbContext> options, IConnectionStringService connectionStringService)
        : base(options)
    {
        _connectionStringService = connectionStringService;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlServer(_connectionStringService.ConnectionString);
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