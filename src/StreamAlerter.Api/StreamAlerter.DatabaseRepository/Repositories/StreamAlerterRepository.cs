using StreamAlerter.Core.Interfaces.DatabaseRepository;
using StreamAlerter.DatabaseRepository.Context;
using System.Collections.Generic;

namespace StreamAlerter.DatabaseRepository.Repositories
{
    public class StreamAlerterRepository : IStreamAlerterDatabaseRepository
    {
        private StreamAlerterContext streamAlerterContext;

        public StreamAlerterRepository(StreamAlerterContext streamAlerterContext)
        {
            this.streamAlerterContext = streamAlerterContext;
        }

        public bool Delete(Entities.Streamer streamer)
        {
            streamAlerterContext.Remove(streamer);
            streamAlerterContext.SaveChanges();
            return true;
        }

        public Entities.Streamer Insert(Entities.Streamer streamer)
        {
            streamAlerterContext.Streamers.Add(streamer);
            streamAlerterContext.SaveChanges();
            return streamer;
        }

        public Entities.Streamer Select(string name)
        {
            return streamAlerterContext.Streamers.Find(name);
        }

        public IEnumerable<Entities.Streamer> SelectAll()
        {
            return streamAlerterContext.Streamers;
        }

        public Entities.Streamer Update(string name, Entities.Streamer streamer)
        {
            var existingStreamer = Select(name);

            existingStreamer.Name = streamer.Name;
            existingStreamer.InLive = streamer.InLive;

            streamAlerterContext.SaveChanges();

            return existingStreamer;
        }
    }
}
