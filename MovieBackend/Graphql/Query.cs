using MovieBackend.Models;
using Character = MovieBackend.Graphql.Types.Character;
using Movie = MovieBackend.Graphql.Types.Movie;

namespace MovieBackend.Graphql
{
    public class Query
    {
        public async Task<Movie?> GetMovieById(
            [Service] MovieRepository movieRepository,
            int id)
        {
            var movie = await movieRepository.GetMovie(id);
            return movie != null ? new Movie
            {
                ID = movie.ID,
                Title = movie.Title,
                Price = movie.Price,
            } : null;
        }
    }
}

