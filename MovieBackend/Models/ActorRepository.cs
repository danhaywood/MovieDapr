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

        public async Task<List<Actor>> GetActors()
        {
            using (ActivitySource.StartActivity(nameof(GetActors), ActivityKind.Client))
            {
                return await _context.Actor.ToListAsync();
            }
        }
        
        public async Task<Actor?> GetActor(int id)
        {
            using (ActivitySource.StartActivity(nameof(GetActor), ActivityKind.Client))
            {
                return await _context.Actor.FindAsync(id);
            }
        }
        
        public async Task<Actor> CreateActor(ActorDto actorDto)
        {
            var actor = new Actor(actorDto);
            using (ActivitySource.StartActivity(nameof(CreateActor), ActivityKind.Client))
            {
                var actorEntry = _context.Actor.Add(actor);
                await _context.SaveChangesAsync();
                return actorEntry.Entity;
            }
        }
        
        public async Task<Actor?> UpdateActor(ActorDto movieDto)
        {
            using (ActivitySource.StartActivity(nameof(UpdateActor), ActivityKind.Client))
            {
                Actor? actor;
                int id;
                
                using (ActivitySource.StartActivity(nameof(UpdateActor) + ".Find", ActivityKind.Client))
                {
                    id = movieDto.ID;
        
                    actor = await _context.Actor.FindAsync(id);
                    if (actor == null)
                    {
                        return null;
                    }
                }

                actor.Name = actor.Name;
                
                using (ActivitySource.StartActivity(nameof(UpdateActor) + ".Save", ActivityKind.Client))
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
        
                    return await GetActor(id);
                }
            }
        }
        
        public async Task<bool> DeleteActor(int id)
        {
        
            using (ActivitySource.StartActivity(nameof(DeleteActor), ActivityKind.Client))
            {
                
                Actor? actor;
                using (ActivitySource.StartActivity(nameof(DeleteActor) + ".Find", ActivityKind.Client))
                {
                    actor = await _context.Actor.FindAsync(id);
                    if (actor == null)
                    {
                        return false;
                    }
                }
                using (ActivitySource.StartActivity(nameof(DeleteActor) + ".Delete", ActivityKind.Client))
                {
                    _context.Actor.Remove(actor);
                    await _context.SaveChangesAsync();
        
                    return true;
                }
            }
        }
        
        private bool ActorExists(int id)
        {
            using (ActivitySource.StartActivity(nameof(ActorExists), ActivityKind.Client))
            {
                return _context.Movie.Any(x => x.ID == id);
            }
        }

        public async Task<Actor?> FindActorByName(string name)
        {
            using (ActivitySource.StartActivity(nameof(FindActorByName), ActivityKind.Client))
            {
                return await _context.Actor.FirstAsync(x => x.Name == name);
            }
        }
    }
}