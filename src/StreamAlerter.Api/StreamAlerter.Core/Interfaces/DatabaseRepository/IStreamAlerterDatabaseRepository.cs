using System.Collections.Generic;

namespace StreamAlerter.Core.Interfaces.DatabaseRepository
{
    public interface IStreamAlerterDatabaseRepository
    {
        public bool Delete(Entities.Streamer streamer);
        public Entities.Streamer Insert(Entities.Streamer streamer);
        public Entities.Streamer Select(string name);

        public IEnumerable<Entities.Streamer> SelectAll();
        public Entities.Streamer Update(string name, Entities.Streamer streamer);
    }
}
