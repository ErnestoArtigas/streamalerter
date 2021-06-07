using System;
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

        // Override of the Equals operator for easier comparison.
        public override bool Equals(object obj)
        {
            if ((obj == null) || !(this.GetType().Equals(obj.GetType())))
                return false;
            Entities.Streamer streamerToCompare = (Entities.Streamer)obj;                    
            // We need to do equals and not == because different cases
            return Name.Equals(streamerToCompare.Name, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
