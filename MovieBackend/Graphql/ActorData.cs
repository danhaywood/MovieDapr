using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieData;

namespace MovieBackend.Graphql
{
    public class ActorData
    {
        public ActorData()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [UseFiltering()]
        [UseSorting()]
        public virtual List<CharacterData> Characters { get; set; } = new List<CharacterData>();

    }
}