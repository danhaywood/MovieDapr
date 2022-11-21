using MovieBackend.Models;

namespace MovieBackend.Graphql
{
    public class Query
    {
        // [UseFirstOrDefault]
        // [UsePaging()]
        [UseProjection()]
        [UseFiltering()]
        [UseSorting()]
        public IQueryable<Movie> GetMovies([Service(ServiceKind.Synchronized)] MovieRepository movieRepository) => movieRepository.GetMovies();

        // [UseFirstOrDefault]
        // [UsePaging()]
        [UseProjection()]
        [UseFiltering()]
        [UseSorting()]
        public IQueryable<Actor> GetActors([Service(ServiceKind.Synchronized)] ActorRepository actorRepository) => actorRepository.GetActors();

        // [UseFirstOrDefault]
        // [UsePaging()]
        [UseProjection()]
        [UseFiltering()]
        [UseSorting()]
        public IQueryable<Character> GetCharacters([Service(ServiceKind.Synchronized)] CharacterRepository characterRepository) => characterRepository.GetCharacters();
    }
}

