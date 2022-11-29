using Microsoft.EntityFrameworkCore;
using MovieBackend.Infra;
using MovieBackend.Infra.ConnStr;

namespace MovieBackend.Domain;

public class MovieDbContext : DbContext
{
    private readonly IConnectionStringService _connectionStringService;

    public MovieDbContext (DbContextOptions<MovieDbContext> options, IConnectionStringService connectionStringService)
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

    public DbSet<Movie> Movie { get; set; } = default!;
    public DbSet<Actor> Actor { get; set; } = default!;
    public DbSet<Character> Character { get; set; } = default!;
        
}