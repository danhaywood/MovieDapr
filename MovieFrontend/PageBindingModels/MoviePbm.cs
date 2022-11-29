using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieClient;
using MovieData;

namespace MovieFrontend.PageBindingModels;

public class MoviePbm
{
    public MoviePbm()
    {
    }

    public MoviePbm(int id, string genre, decimal price, string title)
    {
        Id = id;
        Genre = genre;
        Price = price;
        Title = title;
    }

    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Release Date")]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    public string Genre { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public List<CharacterPbm> Characters { get; set; } = new();

    public MovieDto AsDto()
    {
        return new MovieDto
        {
            Id = this.Id,
            Genre = Genre,
            Price = Price,
            Title = Title,
            ReleaseDate = ReleaseDate
        };
    }
}

public static class PbmExtension
{

    public static MoviePbm AsPbm(this IMovie_by_id_Movies x)
    {
        return new MoviePbm
        {
            Id = x.Id,
            Title = x.Title,
            Genre = x.Genre,
            Price = x.Price,
            ReleaseDate = x.ReleaseDate.Date
        };
    }
}