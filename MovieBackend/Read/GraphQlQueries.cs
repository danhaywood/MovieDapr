using MovieBackend.Read.Views;

namespace MovieBackend.Read;

public class Query
{
    // [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MovieView> GetMovies([Service(ServiceKind.Synchronized)] MovieViewDbContext context) => context.MovieData;

    // [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ActorView> GetActors([Service(ServiceKind.Synchronized)] MovieViewDbContext context) => context.ActorData;

    // [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<CharacterView> GetCharacters([Service(ServiceKind.Synchronized)] MovieViewDbContext context) => context.CharacterData;
}