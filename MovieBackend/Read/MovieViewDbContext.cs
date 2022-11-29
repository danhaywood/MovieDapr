using Microsoft.EntityFrameworkCore;
using MovieBackend.Infra.ConnStr;
using MovieBackend.Read.Views;

namespace MovieBackend.Read;

public class MovieViewDbContext : DbContext
{
    private readonly IConnectionStringService _connectionStringService;

    public MovieViewDbContext (DbContextOptions<MovieViewDbContext> options, IConnectionStringService connectionStringService)
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("graphql");
            
        modelBuilder.Entity<MovieView>()    .ToView("MovieData");
        modelBuilder.Entity<ActorView>()    .ToView("ActorData");
        modelBuilder.Entity<CharacterView>().ToView("CharacterData");
    }

    public DbSet<MovieView> MovieData { get; set; } = default!;
    public DbSet<ActorView> ActorData { get; set; } = default!;
    public DbSet<CharacterView> CharacterData { get; set; } = default!;
}