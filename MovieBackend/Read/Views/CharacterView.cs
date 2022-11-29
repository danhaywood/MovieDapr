namespace MovieBackend.Read.Views;

public class CharacterView
{
    public int Id { get; set; }
        
    [GraphQLIgnore]
    public int MovieId { get; set; }
        
    public virtual MovieView Movie { get; set; } = null!;

    [GraphQLIgnore]
    public int ActorId { get; set; }
        
    public virtual ActorView Actor { get; set; } = null!;

    public string CharacterName { get; set; } = string.Empty;

}