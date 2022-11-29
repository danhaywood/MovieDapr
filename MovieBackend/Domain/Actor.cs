using MovieBackend.Models;
using MovieData;

namespace MovieBackend.Domain;

public class Actor
{
    public Actor()
    {
    }

    public Actor(ActorDto actorDto)
    {
        Id = actorDto.Id;
        Name = actorDto.Name;
    }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual List<Character> Characters { get; set; } = new List<Character>();

    public ActorDto AsDto()
    {
        return new ActorDto()
        {
            Id = Id,
            Name = Name,
        };
    }
}