﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieData;

namespace MovieBackend.Models
{
    public class Actor
    {
        public Actor()
        {
        }

        public Actor(ActorDto actorDto)
        {
            ID = actorDto.ID;
            Name = actorDto.Name;
        }

        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;

        [UseFiltering()]
        [UseSorting()]
        public virtual List<Character> Characters { get; set; } = new List<Character>();

        [GraphQLIgnore]
        public ActorDto AsDto()
        {
            return new ActorDto()
            {
                ID = ID,
                Name = Name,
            };
        }
    }
}