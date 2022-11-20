using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api;

namespace MovieFrontend.Models
{
    public class Movie
    {
        public Movie()
        {
        }

        public Movie(MovieDto movieDto)
        {
            ID = movieDto.ID;
            Title = movieDto.Title;
            ReleaseDate = movieDto.ReleaseDate;
            Genre = movieDto.Genre;
        }

        public int ID { get; set; }
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public MovieDto AsDto()
        {
            return new MovieDto()
            {
                ID = ID,
                Title = Title,
                ReleaseDate = ReleaseDate,
                Genre = Genre,
                Price = Price,
            };
        }
    }
}