using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieData;

namespace MovieBackend.Graphql.Types
{
    public class Actor
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual List<Character> Characters { get; set; } = new List<Character>();
    }
}