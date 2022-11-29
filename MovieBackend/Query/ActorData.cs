using MovieBackend.Graphql;

namespace MovieBackend.Query;

public class ActorData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [UseFiltering()]
    [UseSorting()]
    public virtual List<CharacterData> Characters { get; set; } = new List<CharacterData>();

}