using MovieBackend.Graphql.Types;

namespace MovieBackend.Graphql
{
    public class Query
    {
        public Movie GetMovie() =>
            new Movie
            {
                Title = "C# in depth.",
                Characters = new List<Character>()
                {
                    new()
                    {
                        CharacterName = "Fred"
                    },
                    new()
                    {
                        CharacterName = "Jones"
                    }
                }
            };
    }
}

