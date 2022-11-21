using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MovieBackend.Controllers;
using MovieBackend.Data;
using MovieData;

namespace MovieBackend.Models
{
    public class ActorRepository
    {
        private static readonly ActivitySource ActivitySource = new(nameof(ActorRepository));
        
        private readonly MovieContext _context;
        public ActorRepository(MovieContext context)
        {
            _context = context;
        }

        public DbSet<Actor> GetActors()
        {
            using (ActivitySource.StartActivity(nameof(GetActors), ActivityKind.Client))
            {
                return _context.Actor;
            }
        }
        
        public async Task<List<Actor>> GetActorsAsync()
        {
            using (ActivitySource.StartActivity(nameof(GetActorsAsync), ActivityKind.Client))
            {
                return await _context.Actor.ToListAsync();
            }
        }
        
        public async Task<Actor?> GetActorAsync(int id)
        {
            using (ActivitySource.StartActivity(nameof(GetActorAsync), ActivityKind.Client))
            {
                return await _context.Actor.FindAsync(id);
            }
        }
        
        public async Task<Actor> CreateActorAsync(ActorDto actorDto)
        {
            var actor = new Actor(actorDto);
            using (ActivitySource.StartActivity(nameof(CreateActorAsync), ActivityKind.Client))
            {
                var actorEntry = _context.Actor.Add(actor);
                await _context.SaveChangesAsync();
                return actorEntry.Entity;
            }
        }
        
        public async Task<Actor?> UpdateActorAsync(ActorDto movieDto)
        {
            using (ActivitySource.StartActivity(nameof(UpdateActorAsync), ActivityKind.Client))
            {
                Actor? actor;
                int id;
                
                using (ActivitySource.StartActivity(nameof(UpdateActorAsync) + ".Find", ActivityKind.Client))
                {
                    id = movieDto.ID;
        
                    actor = await _context.Actor.FindAsync(id);
                    if (actor == null)
                    {
                        return null;
                    }
                }

                actor.Name = actor.Name;
                
                using (ActivitySource.StartActivity(nameof(UpdateActorAsync) + ".Save", ActivityKind.Client))
                {
                    _context.Attach(actor).State = EntityState.Modified;
        
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException) when (!ActorExists(id))
                    {
                        return null;
                    }
        
                    return await GetActorAsync(id);
                }
            }
        }
        
        public async Task<bool> DeleteActorAsync(int id)
        {
        
            using (ActivitySource.StartActivity(nameof(DeleteActorAsync), ActivityKind.Client))
            {
                
                Actor? actor;
                using (ActivitySource.StartActivity(nameof(DeleteActorAsync) + ".Find", ActivityKind.Client))
                {
                    actor = await _context.Actor.FindAsync(id);
                    if (actor == null)
                    {
                        return false;
                    }
                }
                using (ActivitySource.StartActivity(nameof(DeleteActorAsync) + ".Delete", ActivityKind.Client))
                {
                    _context.Actor.Remove(actor);
                    await _context.SaveChangesAsync();
        
                    return true;
                }
            }
        }
        
        public async Task<Actor?> FindActorByNameAsync(string name)
        {
            using (ActivitySource.StartActivity(nameof(FindActorByNameAsync), ActivityKind.Client))
            {
                return await _context.Actor.FirstAsync(x => x.Name == name);
            }
        }

        private bool ActorExists(int id)
        {
            using (ActivitySource.StartActivity(nameof(ActorExists), ActivityKind.Client))
            {
                return _context.Movie.Any(x => x.ID == id);
            }
        }
    }
}