using MovieData;

namespace MovieBackend.Models
{
    public class Character
    {
        public int Id { get; set; }
        
        [GraphQLIgnore]
        public int MovieId { get; set; }
        
        public virtual Movie Movie { get; set; }
        
        [GraphQLIgnore]
        public int ActorId { get; set; }
        
        public virtual Actor Actor { get; set; }

        public string CharacterName { get; set; } = string.Empty;

        [GraphQLIgnore]
        public CharacterDto AsDto()
        {
            return new CharacterDto()
            {
                ID = Id,
                CharacterName = CharacterName,
            };
        }
    }
}