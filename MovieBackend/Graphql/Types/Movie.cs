using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieData;

namespace MovieBackend.Graphql.Types
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; } = string.Empty;

        public DateTime ReleaseDate { get; set; }

        public string Genre { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public virtual List<Character> Characters { get; set; } = new List<Character>();
        
    }
}