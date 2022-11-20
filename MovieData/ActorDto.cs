using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieData
{
    public class ActorDto
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}