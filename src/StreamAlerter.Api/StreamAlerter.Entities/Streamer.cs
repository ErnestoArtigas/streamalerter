using System.ComponentModel.DataAnnotations;

namespace StreamAlerter.Entities
{
    public class Streamer
    {
        [Key]
        [MaxLength(50)]
        public string Name { get; set; }
        public bool InLive { get; set; }

        // The default constructor must be explicitely written because we have a constructor with parameter.
        public Streamer() { }

        public Streamer(string streamerName, bool inLive)
        {
            this.Name = streamerName;
            this.InLive = inLive;
        }
    }
}
