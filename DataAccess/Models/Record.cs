using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Record
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public uint T { get; set; }
        public decimal V { get; set; }
    }
}
