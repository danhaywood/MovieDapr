namespace MovieBackend.Read.Views;

public class ActorView
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [UseFiltering()]
    [UseSorting()]
    public virtual List<CharacterView> Characters { get; set; } = new List<CharacterView>();

}