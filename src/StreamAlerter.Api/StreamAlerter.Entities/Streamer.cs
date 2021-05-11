using System.ComponentModel.DataAnnotations;

namespace StreamAlerter.Entities
{
    public class Streamer
    {
        [Key]
        public string Name { get; set; }
    }
}
