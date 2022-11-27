using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieFrontend.PageBindingModels
{
    public class CharacterPbm
    {
        public int ID { get; set; }
        public string CharacterName { get; set; } = string.Empty;
    }
}