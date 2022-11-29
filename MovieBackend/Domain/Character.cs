using MovieBackend.Models;
using MovieData;

namespace MovieBackend.Domain;

public class Character
{
    public int Id { get; set; }
        
    public int MovieId { get; set; }
    public virtual Movie Movie { get; set; } = null!;

    public int ActorId { get; set; }
    public virtual Actor Actor { get; set; } = null!;

    public string CharacterName { get; set; } = string.Empty;

    public CharacterDto AsDto()
    {
        return new CharacterDto()
        {
            ID = Id,
            CharacterName = CharacterName,
        };
    }
}