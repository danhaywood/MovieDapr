using Dapr.Client;
using Microsoft.EntityFrameworkCore;
using MovieBackend.Models;
using MovieBackend.Services;

namespace MovieBackend.Data
{
    public class MovieDbContext : DbContext
    {
        private readonly ConnectionStringService _connectionStringService;

        public MovieDbContext (DbContextOptions<MovieDbContext> options, ConnectionStringService connectionStringService)
            : base(options)
        {
            _connectionStringService = connectionStringService;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(_connectionStringService.GetConnectionString());
        }

        public DbSet<Movie> Movie { get; set; } = default!;
        public DbSet<Actor> Actor { get; set; } = default!;
        public DbSet<Character> Character { get; set; } = default!;
        
    }
}
