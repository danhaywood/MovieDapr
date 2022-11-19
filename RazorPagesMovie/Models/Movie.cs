using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RazorPagesMovie.Dto;

namespace RazorPagesMovie.Models
{
    public class Movie
    {
        public Movie()
        {
        }

        public Movie(MovieDto movieDto)
        {
            this.ID = movieDto.ID;
            this.Title = movieDto.Title;
            this.ReleaseDate = movieDto.ReleaseDate;
            this.Genre = movieDto.Genre;
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
                ID = this.ID,
                Title = this.Title,
                ReleaseDate = this.ReleaseDate,
                Genre = this.Genre,
                Price = this.Price,
            };
        }
    }
}