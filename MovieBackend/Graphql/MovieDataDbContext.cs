using Microsoft.EntityFrameworkCore;
using MovieBackend.Infra.ConnStr;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("graphql");
            
        modelBuilder.Entity<MovieData>()    .ToView("MovieData");
        modelBuilder.Entity<ActorData>()    .ToView("ActorData");
        modelBuilder.Entity<CharacterData>().ToView("CharacterData");
    }

    public DbSet<MovieData> MovieData { get; set; } = default!;
    public DbSet<ActorData> ActorData { get; set; } = default!;
    public DbSet<CharacterData> CharacterData { get; set; } = default!;
}