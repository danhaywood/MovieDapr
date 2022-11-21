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

        public async Task<List<Character>> GetCharacters()
        {
            using (ActivitySource.StartActivity(nameof(GetCharacters), ActivityKind.Client))
            {
                return await _context.Character.ToListAsync();
            }
        }
        
        public async Task<Character?> GetCharacter(int id)
        {
            using (ActivitySource.StartActivity(nameof(GetCharacter), ActivityKind.Client))
            {
                return await _context.Character.FindAsync(id);
            }
        }
        
        private bool MovieExists(int id)
        {
            using (ActivitySource.StartActivity(nameof(MovieExists), ActivityKind.Client))
            {
                return _context.Movie.Any(x => x.ID == id);
            }
        }
    }
}