using Microsoft.EntityFrameworkCore;
using MovieBackend.Data;
using MovieBackend.Models;

namespace MovieBackend.Services;

public class SeedDataService : IBootstrappable
{
    private readonly MovieDbContext _dbContext;

    public SeedDataService(MovieDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public void Bootstrap()
    {
        RemoveAll(_dbContext.Character);
        RemoveAll(_dbContext.Actor);
        RemoveAll(_dbContext.Movie);

        ResetMovies();
        ResetActors();
        _dbContext.SaveChanges();
        
        ResetCharacters();
        _dbContext.SaveChanges();
    }

    private void RemoveAll<T>(DbSet<T> dbSet) where T : class
    {
        List<T> entities = dbSet.ToList();
        dbSet.RemoveRange(entities);
    }

    private void ResetMovies()
    {
        _dbContext.Movie.AddRange(
            new Movie
            {
                Title = "When Harry Met Sally",
                ReleaseDate = DateTime.Parse("1989-2-12"),
                Genre = "Romantic Comedy",
                Price = 7.99M
            },
            new Movie
            {
                Title = "Ghostbusters ",
                ReleaseDate = DateTime.Parse("1984-3-13"),
                Genre = "Comedy",
                Price = 8.99M
            },
            new Movie
            {
                Title = "Groundhog Day",
                ReleaseDate = DateTime.Parse("1997-5-7"),
                Genre = "Comedy",
                Price = 9.99M
            }
        );
    }
    
    private void ResetActors()
    {
        _dbContext.Actor.AddRange(
            new Actor
            {
                Name = "Meg Ryan"
            },
            new Actor
            {
                Name = "Billy Crystal"
            },
            new Actor
            {
                Name = "Carrie Fisher"
            },
            new Actor
            {
                Name = "Bruno Kirby"
            },
            new Actor
            {
                Name = "Bill Murray"
            },
            new Actor
            {
                Name = "Andie MacDowell"
            },
            new Actor
            {
                Name = "Harold Ramis"
            },
            new Actor
            {
                Name = "Dan Ackroyd"
            },
            new Actor
            {
                Name = "Ernie Hudson"
            }
        );
    }
    private void ResetCharacters()
    {
        NewCharacter("When Harry Met Sally", "Meg Ryan", "Sally Albright");
        NewCharacter("When Harry Met Sally", "Billy Crystal", "Harry Burns");
        NewCharacter("When Harry Met Sally", "Carrie Fisher", "Marie");
        NewCharacter("When Harry Met Sally", "Bruno Kirby", "Jess");
        NewCharacter("Groundhog Day", "Bill Murray", "Phil");
        NewCharacter("Groundhog Day", "Andie Macdowell", "Rita");
        NewCharacter("Groundhog Day", "Harold Ramis", "Neurologist");
        NewCharacter("Ghostbusters", "Bill Murray", "Peter Venkman");
        NewCharacter("Ghostbusters", "Harold Ramis", "Egon Spengler");
        NewCharacter("Ghostbusters", "Dan Ackroyd", "Ray Stanz");
        NewCharacter("Ghostbusters", "Ernie Hudson", "Winston Zeddemore");
    }

    private Character NewCharacter(string movieTitle, string actorName, string characterName)
    {
        var movie = MovieFor(movieTitle);
        var actor = ActorFor(actorName);
        
        var character = new Character()
        {
            Movie = movie,
            Actor = actor,
            CharacterName = characterName
        };
        movie.Characters.Add(character);
        actor.Characters.Add(character);
        
        return character;
    }
    private Movie MovieFor(string title)
    {
        return _dbContext.Movie.FirstOrDefault(x => x.Title == title)!;
    }
    private Actor ActorFor(string name)
    {
        return _dbContext.Actor.FirstOrDefault(x => x.Name == name)!;
    }
}