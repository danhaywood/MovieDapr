using MovieData;

namespace MovieBackend.Models
{
    public class Character
    {
        public Character()
        {
        }

        public Character(CharacterDto characterDto)
        {
            ID = characterDto.ID;
            // MovieID = characterDto.MovieID;
            // ActorID = characterDto.ActorID;
            CharacterName = characterDto.CharacterName;
        }

        public int ID { get; set; }
        
        public int MovieID { get; set; }
        public Movie Movie { get; set; }
        
        public int ActorID { get; set; }
        public Actor Actor { get; set; }

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