using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models
{
    public class Record
    {
        [Key][JsonIgnore]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public uint T { get; set; } // todo should use JsonPropertyName attribute 
        public decimal V { get; set; }
    }
}
