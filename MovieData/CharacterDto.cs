using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieData;

public class CharacterDto
{
    public int ID { get; set; }
    public string CharacterName { get; set; } = string.Empty;
}