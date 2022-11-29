using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MovieBackend.Controllers;
using MovieBackend.Domain;
using MovieData;

namespace MovieBackend.Models;

public class CharacterRepository
{
    private static readonly ActivitySource ActivitySource = new(nameof(CharacterRepository));
        
    private readonly MovieDbContext _dbContext;
    public CharacterRepository(MovieDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public DbSet<Character> GetCharacters()
    {
        using (ActivitySource.StartActivity(nameof(GetCharacters), ActivityKind.Client))
        {
            return _dbContext.Character;
        }
    }
        
    public async Task<List<Character>> GetCharactersAsync()
    {
        using (ActivitySource.StartActivity(nameof(GetCharactersAsync), ActivityKind.Client))
        {
            return await _dbContext.Character.ToListAsync();
        }
    }
        
    public async Task<Character?> GetCharacterAsync(int id)
    {
        using (ActivitySource.StartActivity(nameof(GetCharacterAsync), ActivityKind.Client))
        {
            return await _dbContext.Character.FindAsync(id);
        }
    }
        
}