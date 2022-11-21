using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MovieBackend.Controllers;
using MovieBackend.Data;
using MovieData;

namespace MovieBackend.Models
{
    public class CharacterRepository
    {
        private static readonly ActivitySource ActivitySource = new(nameof(CharacterRepository));
        
        private readonly MovieContext _context;
        public CharacterRepository(MovieContext context)
        {
            _context = context;
        }

        public DbSet<Character> GetCharacters()
        {
            using (ActivitySource.StartActivity(nameof(GetCharacters), ActivityKind.Client))
            {
                return _context.Character;
            }
        }
        
        public async Task<List<Character>> GetCharactersAsync()
        {
            using (ActivitySource.StartActivity(nameof(GetCharactersAsync), ActivityKind.Client))
            {
                return await _context.Character.ToListAsync();
            }
        }
        
        public async Task<Character?> GetCharacterAsync(int id)
        {
            using (ActivitySource.StartActivity(nameof(GetCharacterAsync), ActivityKind.Client))
            {
                return await _context.Character.FindAsync(id);
            }
        }
        
    }
}