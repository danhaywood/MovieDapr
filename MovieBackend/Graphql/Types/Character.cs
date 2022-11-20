using MovieData;

namespace MovieBackend.Graphql.Types
{
    public class Character
    {
        public int ID { get; set; }
        
        public virtual Movie Movie { get; set; }
        
        public virtual Actor Actor { get; set; }

        public string CharacterName { get; set; } = string.Empty;

    }
}