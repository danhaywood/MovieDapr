namespace MovieBackend.Graphql
{
    public class Query
    {
        // [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<MovieData> GetMovies([Service(ServiceKind.Synchronized)] MovieDataDbContext context) => context.MovieData;

        // [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ActorData> GetActors([Service(ServiceKind.Synchronized)] MovieDataDbContext context) => context.ActorData;

        // [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<CharacterData> GetCharacters([Service(ServiceKind.Synchronized)] MovieDataDbContext context) => context.CharacterData;
    }

}

