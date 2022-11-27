using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieData;

namespace MovieBackend.Models
{
    public class Movie
    {
        public Movie()
        {
        }

        public Movie(MovieDto movieDto)
        {
            Id = movieDto.Id;
            Title = movieDto.Title;
            ReleaseDate = movieDto.ReleaseDate;
            Genre = movieDto.Genre;
        }

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
        public virtual List<Character> Characters { get; set; } = new List<Character>();
        
        [GraphQLIgnore]
        public MovieDto AsDto()
        {
            return new MovieDto()
            {
                Id = Id,
                Title = Title,
                ReleaseDate = ReleaseDate,
                Genre = Genre,
                Price = Price,
                Characters = Characters.Select(x => x.AsDto()).ToList()
            };
        }
    }
}