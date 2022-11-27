using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieData;

namespace MovieBackend.Graphql
{
    public class MovieData
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [UseFiltering()]
        [UseSorting()]
        public virtual List<CharacterData> Characters { get; set; } = new List<CharacterData>();
        
    }
}