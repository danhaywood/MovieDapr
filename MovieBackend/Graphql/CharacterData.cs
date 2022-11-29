using MovieData;

namespace MovieBackend.Graphql;

public class CharacterData
{
    public int Id { get; set; }
        
    [GraphQLIgnore]
    public int MovieId { get; set; }
        
    public virtual MovieData Movie { get; set; } = null!;

    [GraphQLIgnore]
    public int ActorId { get; set; }
        
    public virtual ActorData Actor { get; set; } = null!;

    public string CharacterName { get; set; } = string.Empty;

}