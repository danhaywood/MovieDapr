using MovieData;

namespace MovieBackend.Models
{
    public class Character
    {
        public Character()
        {
        }

        public int ID { get; set; }
        
        public int MovieID { get; set; }
        public virtual Movie Movie { get; set; }
        
        public int ActorID { get; set; }
        public virtual Actor Actor { get; set; }

        public string CharacterName { get; set; } = string.Empty;

        public CharacterDto AsDto()
        {
            return new CharacterDto()
            {
                ID = ID,
                CharacterName = CharacterName,
            };
        }
    }
}